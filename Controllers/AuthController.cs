using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Data;
using mini_project_csharp.Models;

namespace mini_project_csharp.Controllers
{
  public class AuthController(ApplicationDbContext context) : Controller
  {
    private readonly ApplicationDbContext _context = context;

        public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return  View();
    }

    public IActionResult RegisterClient(Client client) {
      if (ModelState.IsValid)
      {
        _context.Add(client);
        _context.SaveChanges();

        return RedirectToAction("Login", "Auth");
      }

      return View("Register", client);
    }
  }
}