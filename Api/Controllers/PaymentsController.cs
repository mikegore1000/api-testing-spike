using System;
using Api.Contracts;
using Api.Domain;
using Microsoft.AspNetCore.Mvc;
using SimpleEventStore;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class PaymentsController : Controller
    {
        private readonly EventStore eventStore;

        public PaymentsController(EventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        [HttpPost("{paymentReference}")]
        public IActionResult Post([FromBody]PaymentIntent intent, string paymentReference)
        {
            if (!ModelState.IsValid)
                return new ValidationErrorResult(ModelState);


            eventStore.AppendToStream(paymentReference, 0, new EventData(Guid.NewGuid(), new PaymentIntentAdded()));

            return Ok();
        }
    }
}
