using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IGreetingService greetingService;

        public TestController(IGreetingService greetingService)
        {
            this.greetingService = greetingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(greetingService.GetGreeting());
        }
    }
}
