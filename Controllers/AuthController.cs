using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Data;
using mini_project_csharp.Models;

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
        // Mapeando os dados da ViewModel para o modelo de banco de dados
            var client = new Client
            {
                Nome = model.Nome,
                Email = model.Email,
                Password = model.Password // Certifique-se de hash/salvar a senha corretamente
            };

            _context.Add(client);
            _context.SaveChanges();

            return RedirectToAction("Login", "Auth");
        }

        // Retorna a mesma View com os erros de validação
        return View("Register", model);
    }

  }
}