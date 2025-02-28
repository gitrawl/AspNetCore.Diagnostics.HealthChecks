using FluentAssertions;
using HealthChecks.RavenDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Xunit;

namespace HealthChecks.RavenDb.Tests.DependencyInjection
{
    public class ravendb_registration_should
    {
        [Fact]
        public void add_health_check_when_properly_configured()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddRavenDB(_ => _.Urls = new[] { "http://localhost:8080", "http://localhost:8081" });

            using var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("ravendb");
            check.GetType().Should().Be(typeof(RavenDBHealthCheck));
        }

        [Fact]
        public void add_named_health_check_when_properly_configured()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddRavenDB(_ => _.Urls = new[] { "http://localhost:8080", "http://localhost:8081" },
                    name: "my-ravendb");

            using var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("my-ravendb");
            check.GetType().Should().Be(typeof(RavenDBHealthCheck));
        }

        [Fact]
        public void add_health_check_when_properly_configured_single_connection()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddRavenDB(setup => setup.Urls = new[] { "http://localhost:8080" });

            using var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("ravendb");
            check.GetType().Should().Be(typeof(RavenDBHealthCheck));
        }

        [Fact]
        public void add_named_health_check_when_properly_configured_single_connection()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddRavenDB(setup => setup.Urls = new[] { "http://localhost:8080" }, name: "my-ravendb");

            using var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("my-ravendb");
            check.GetType().Should().Be(typeof(RavenDBHealthCheck));
        }
    }
}
