using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Web.Models;

namespace AuthenticationService.Web.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
