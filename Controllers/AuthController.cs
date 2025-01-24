using System.Security.Claims;
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

        //INJEÇÃO DE DEPENDÊNCIA
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        //MÉTODO QUE ABRE O FORMULÁRIO DE LOGIN
        public IActionResult Login()
        {
            return View();
        }

        //MÉTODO QUE ABRE O FORMULÁRIO DE REGISTO
        public IActionResult Register()
        {
            return View();
        }

        //MÉTODO RESPONSÁVEL POR REGISTAR UM UTILIZAR NA BASE DE DADOS E NA APLICAÇÃO
        [HttpPost]
        public IActionResult RegisterClient(RegisterViewModel model)
        {
            if (ModelState.IsValid) //VERIFICA SE OS DADOS SÃO VÁLIDOS
            {
                //FAZ A ENCRIPTAÇÃO DA PASSWORD
                var passwordService = new PasswordService();
                var hashedPassword = passwordService.HashPassword(model.Password);

                var client = new Client
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Password = hashedPassword,
                };
                
                //SALVA O UTILIZADOR NA BASE DE DADOS
                _context.Add(client);
                _context.SaveChanges();

                return RedirectToAction("Login", "Auth"); //REDIRECIONA O UTILIZADOR PARA A PÁGINA DE LOGIN
            }
            return View("Register", model);
        }

        //MÉTODO RESPONSÁVEL POR INICIAR A SESSÃO DO UTILIZAROR
        [HttpPost]
        public async Task<IActionResult> LoginClient(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //BUSCA O UTILIZADOR NA BASE DE DADOS
                var user = _context.Clientes.FirstOrDefault(c => c.Email == model.Email);
                
                //VERIFICA A EXISTÊNCIA DO UTILIZADOR
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Este utilizador não existe.");
                }
                else
                {
                    var passwordService = new PasswordService();
                    var userIdString = user.IdClientes.ToString();

                    //DESENCRIPTA A PASSWORD E VERIFICA SE SÃO IGUAIS
                    if (passwordService.VerifyPassword(user.Password, model.Password))
                    {
                        //ATRIBUI ESSES DADOS A UMA CLAIM PARA INICIAR A SESSÃO DO UTILIZADOR
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Name, user.Nome),
                            new(ClaimTypes.Email, user.Email),
                            new(ClaimTypes.NameIdentifier, userIdString),
                        };
                        
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        
                        return RedirectToAction("Index", "Home"); //REDIRECIONA O UTILIZADOR PARA A PÁGINA HOME
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Password), "A password está incorreta. Tente novamente.");
                    }
                }
            }
            
            return View("Login", model);
        }

        //MÉTODO PARA ENCERRAR A SESSÃO DO UTILIZADOR
        public async Task<IActionResult> Logout()
        {
            //LIMPA/APAGA AS COOKIES COM A SESSÃO DO CLIENTE
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth"); //REDIRECIONA O MESMO PARA A PÁGINA DE LOGIN
        }
    }
}