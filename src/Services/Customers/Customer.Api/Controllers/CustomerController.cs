using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Success" });
        }
    }
}
