using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace mini_project_csharp.Controllers
{
    public class HomeController : Controller
    {
        //MÉTODO PARA ABRIR A VIEW DA PÁGINA INICIAL
        public IActionResult Index()
        {
            return View();
        }
    }
}