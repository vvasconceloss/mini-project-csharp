using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Models;

namespace mini_project_csharp.Controllers
{
    public class DeleteController : Controller
{
    public IActionResult Index()
        {
            return View(); // Isso renderiza a view em Views/Edit/Index.cshtml
        }
}

}