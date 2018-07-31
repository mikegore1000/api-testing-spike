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
        [Fact]
        public async Task Test()
        {
            var factory = new WebApplicationFactory<Startup>();
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
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IGreetingService, AnotherFakeGreetingService>();
            });
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
