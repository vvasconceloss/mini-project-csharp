using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Data;
using mini_project_csharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mini_project_csharp.Controllers
{
  [Authorize]
  public class ClientController : Controller
  {
    private readonly ApplicationDbContext _context;
    
    public ClientController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      var clients = _context.Clientes.ToList();
      return View(clients);
    }

    public IActionResult Edit()
    {
      return View();
    }

    public IActionResult Delete()
    {
      return View();
    }

    [HttpGet]
    public IActionResult Add()
    {
      var codPostais = _context.CodPostals.Select(c => new SelectListItem
      {
        Value = c.IdCodPostal.ToString(),
        Text = c.Codpostal + " - " + c.Localidade
      }).ToList();

      ViewBag.CodPostais = codPostais;

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(Client newClient)
    {
      if (!ModelState.IsValid)
      {
        var codPostais = _context.CodPostals.Select(c => new SelectListItem
        {
          Value = c.IdCodPostal.ToString(),
          Text = c.Codpostal + " - " + c.Localidade
        }).ToList();

        ViewBag.CodPostais = codPostais;

        return View(newClient);
      }
      
      var client = new Client
      {
        Nome = newClient.Nome,
        Apelido = newClient.Apelido,
        Endereco = newClient.Endereco,
        Telefone = newClient.Telefone,
        Nif = newClient.Nif,
        Email = newClient.Email,
        Password = newClient.Password,
        IdCodPostal = newClient.IdCodPostal
      };

      _context.Clientes.Add(newClient);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }
  }
}
