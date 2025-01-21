using Microsoft.AspNetCore.Mvc;

namespace mini_project_csharp.Controllers
{
  public class ClientController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Edit()
    {
      return View();
    }

    public IActionResult Delete()
    {
      return View();
    }
  }
}