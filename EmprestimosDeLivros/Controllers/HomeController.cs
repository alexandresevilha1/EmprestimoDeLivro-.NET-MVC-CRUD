using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmprestimosDeLivros.Models;
using EmprestimosDeLivros.Services.SessaoService;

namespace EmprestimosDeLivros.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISessaoInterface _sessaoInterface;

    public HomeController(ILogger<HomeController> logger, ISessaoInterface sessaoInterface)
    {
        _logger = logger;
        _sessaoInterface = sessaoInterface;
    }

    public IActionResult Index()
    {
        var usuario = _sessaoInterface.BuscarSessao();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Login");
        }
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
