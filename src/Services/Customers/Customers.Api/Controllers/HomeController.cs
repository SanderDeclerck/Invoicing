using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
