using System.Security.Claims; // Para trabalhar com claims de autenticação
using mini_project_csharp.Data;
using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Models;
using mini_project_csharp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace mini_project_csharp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Construtor que recebe o contexto da base de dados
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Mostra a página de login
        public IActionResult Login()
        {
            return View();
        }

        // Mostra a página de registo
        public IActionResult Register()
        {
            return View();
        }

        // Regista um novo cliente no sistema
        [HttpPost]
        public IActionResult RegisterClient(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Verifica se os dados fornecidos no formulário estão corretos
            {
                var passwordService = new PasswordService();
                var hashedPassword = passwordService.HashPassword(model.Password); // Encripta a palavra-passe

                var client = new Client
                {
                    Nome = model.Nome, 
                    Email = model.Email, 
                    Password = hashedPassword 
                };

                _context.Add(client); // Adiciona o cliente à base de dados
                _context.SaveChanges(); // Salva as alterações

                return RedirectToAction("Login", "Auth"); // Redireciona para a página de login
            }
            return View("Register", model); // Volta para a página de registo, caso os dados sejam inválidos
        }

        // Faz login do cliente
        [HttpPost]
        public async Task<IActionResult> LoginClient(LoginViewModel model)
        {
            if (ModelState.IsValid) // Verifica se os dados fornecidos no formulário estão corretos
            {
                var user = _context.Clientes.FirstOrDefault(c => c.Email == model.Email); // Procura o cliente pelo email

                if (user == null) // Se não encontrar o cliente, exibe um erro
                {
                    ModelState.AddModelError(nameof(model.Email), "Este utilizador não existe.");
                }
                else
                {
                    var passwordService = new PasswordService();

                    // Verifica se a palavra-passe fornecida está correta
                    if (passwordService.VerifyPassword(user.Password, model.Password))
                    {
                        // Cria as claims (informações do utilizador)
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Name, user.Nome), 
                            new(ClaimTypes.Email, user.Email) 
                        };

                        // Cria a identidade para autenticação
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)); // Faz o login do utilizador

                        return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Password), "A password está incorreta. Tente novamente."); // Se a palavra-passe estiver errada, mostra um erro
                    }
                }
            }

            return View("Login", model); // Volta para a página de login, caso algo esteja errado
        }

        // Faz logout do cliente
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Termina a sessão do cliente
            return RedirectToAction("Login", "Auth"); // Redireciona para a página de login
        }
    }
}
