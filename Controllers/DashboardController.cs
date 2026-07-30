using Microsoft.AspNetCore.Mvc;

namespace TiendaApp.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}