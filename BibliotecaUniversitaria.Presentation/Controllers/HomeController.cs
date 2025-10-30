using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BibliotecaUniversitaria.Presentation.Models;

namespace BibliotecaUniversitaria.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.Title = "Sistema de Gestão de Biblioteca Universitária";
        ViewBag.Message = "Bem-vindo ao sistema de gestão de biblioteca universitária!";
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
