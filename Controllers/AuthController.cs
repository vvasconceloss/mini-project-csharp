using System.Security.Claims;
using mini_project_csharp.Data;
using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace mini_project_csharp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterClient(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Password = model.Password
                };

                _context.Add(client);
                _context.SaveChanges();

                return RedirectToAction("Login", "Auth");
            }
            return View("Register", model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginClient(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Clientes.FirstOrDefault(c => c.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Este utilizador não existe.");
                }
                else
                {
                    if (user != null && user.Password == model.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Name, user.Nome),
                            new(ClaimTypes.Email, user.Email)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Password), "A password está incorreta. Tente novamente.");
                    }
                }
            }

            return View("Login", model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}