using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IGreetingService greetingService;
        private readonly IDatabase database;

        public TestController(IGreetingService greetingService, IDatabase database)
        {
            this.greetingService = greetingService;
            this.database = database;
        }

        [HttpGet]
        public IActionResult Get()
        {
            database.Save();

            return Ok(greetingService.GetGreeting());
        }
    }
}
