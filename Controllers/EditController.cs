using Microsoft.AspNetCore.Mvc;

namespace mini_project_csharp.Controllers
{
    public class EditController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Isso renderiza a view em Views/Edit/Index.cshtml
        }
    }
}

