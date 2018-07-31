using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Tests
{
    public class TestControllerTests
    {
        // With standard factory - the DI container services need to be overridden on a per-test basis
        [Fact]
        public async Task Test()
        {
            var factory = new WebApplicationFactory<Startup>();
            var anotherFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IGreetingService, FakeGreetingService>();
                    services.AddSingleton<IDatabase, FakeDatabase>();
                });
            });

            var client = anotherFactory.CreateClient();

            var result = await client.GetStringAsync("/api/test");

            Assert.Equal("Fake", result);
        }



        // Proves that this will only override the services here rather than everything in the container as the fake database is still used
        [Fact]
        public async Task TestWithApiFactory()
        {
            var factory = new ApiFactory();
            var anotherFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IGreetingService, FakeGreetingService>();
                });
            });

            var client = anotherFactory.CreateClient();

            var result = await client.GetStringAsync("/api/test");

            Assert.Equal("Fake", result);
        }
    }

    public class ApiFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            // This ConfigureServices won't override dependencies setup in the Startup class (i.e. Startup will replace them and therefore the ProperGreetingService is used) - need to use ConfigureTestServices for that
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IGreetingService, AnotherFakeGreetingService>();
                services.AddSingleton<IDatabase, FakeDatabase>(); // This is used as it isn't defined
            });
        }
    }

    public class FakeDatabase : IDatabase
    {
        public void Save()
        {
        }
    }

    public class FakeGreetingService : IGreetingService
    {
        public string GetGreeting()
        {
            return "Fake";
        }
    }

    public class AnotherFakeGreetingService : IGreetingService
    {
        public string GetGreeting()
        {
            return "Another Fake";
        }
    }
}
