using Microsoft.AspNetCore.Mvc;

namespace mini_project_csharp.Controllers
{
  public class AuthController : Controller
  {
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return  View();
    }
  }
}