using System;
using System.Threading.Tasks;
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
                //builder.ConfigureServices(services =>
                //{
                //    services.AddSingleton<IGreetingService, FakeGreetingService>();
                //});

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

    public class FakeGreetingService : IGreetingService
    {
        public string GetGreeting()
        {
            return "Fake";
        }
    }
}
