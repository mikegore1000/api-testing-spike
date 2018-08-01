using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Contracts;
using Api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimpleEventStore;
using SimpleEventStore.InMemory;
using Xunit;

namespace Api.Tests
{
    public class PaymentControllerTests
    {
        [Fact]
        public async Task when_an_invalid_transaction_amount_is_provided_it_is_rejected()
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var paymentReference = Guid.NewGuid().ToString("D");

            var paymentIntent = new PaymentIntent(
                new TransactionAmount(-100m, "GBP")
            );


            var result = await client.PostAsJsonAsync($"/payments/{paymentReference}", paymentIntent);
            var jsonBody = await result.Content.ReadAsStringAsync();

            var validationResult = JsonConvert.DeserializeObject<ValidationError[]>(jsonBody);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(validationResult);
            Assert.Equal("Transaction.Amount", validationResult.First().ParameterName);
            Assert.Equal("InvalidTransactionAmount", validationResult.First().ErrorCode);
            Assert.NotEmpty(validationResult.First().ErrorMessage);
        }

        [Fact]
        public async Task when_a_valid_intent_is_provided_it_is_successful_and_saved_to_the_database()
        {
            var eventStore = new EventStore(new InMemoryStorageEngine());

            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(configuration =>
                {
                    configuration.ConfigureTestServices(services => services.AddSingleton(eventStore));
                });

            var client = factory.CreateClient();

            var paymentReference = Guid.NewGuid().ToString("D");

            var paymentIntent = new PaymentIntent(
                new TransactionAmount(100m, "GBP")
            );

            var result = await client.PostAsJsonAsync($"/payments/{paymentReference}", paymentIntent);
            var stream = await eventStore.ReadStreamForwards(paymentReference);
            
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(stream);
            Assert.IsType<PaymentIntentAdded>(stream.Single().EventBody);
        }
    }
}
