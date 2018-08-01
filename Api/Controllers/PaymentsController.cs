using Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class PaymentsController : Controller
    {

        [HttpPost("{paymentReference}")]
        public IActionResult Post([FromBody]PaymentIntent intent, string paymentReference)
        {
            if (!ModelState.IsValid)
                return new ValidationErrorResult(ModelState);

            return Ok();
        }
    }
}
