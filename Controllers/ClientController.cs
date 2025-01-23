using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Data;
using mini_project_csharp.Models;
using mini_project_csharp.Services;
using Microsoft.EntityFrameworkCore;
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
      var clients = _context.Clientes.Include(c => c.CodPostal).ToList();
      return View(clients);
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
          Text = c.CodPostalFormatado
        }).ToList();

        ViewBag.CodPostais = codPostais;

        return View(newClient);
      }

      var passwordService = new PasswordService();
      newClient.Password = passwordService.HashPassword(newClient.Password);

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

    [HttpGet]
    public IActionResult Edit(int id)
    {
      Console.WriteLine(id);

      var client = _context.Clientes.Find(id);

      if (client == null)
      {
        return NotFound();
      }
      
      var codPostais = _context.CodPostals.Select(c => new SelectListItem
      {
        Value = c.IdCodPostal.ToString(),
        Text = c.CodPostalFormatado
      }).ToList();
      
      ViewBag.CodPostais = codPostais;

      return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Client updatedClient)
    {
      if (!ModelState.IsValid)
      {
        var codPostais = _context.CodPostals.Select(c => new SelectListItem
        {
          Value = c.IdCodPostal.ToString(),
          Text = c.CodPostalFormatado
          }).ToList();
          
          ViewBag.CodPostais = codPostais;
          return View(updatedClient);
      }

      var client = _context.Clientes.Find(updatedClient.IdClientes);
      
      if (client == null)
      {
        return NotFound();
      }

      client.Nome = updatedClient.Nome;
      client.Apelido = updatedClient.Apelido;
      client.Endereco = updatedClient.Endereco;
      client.Telefone = updatedClient.Telefone;
      client.Nif = updatedClient.Nif;
      client.Email = updatedClient.Email;
      
      if (!string.IsNullOrEmpty(updatedClient.Password))
      {
        var passwordService = new PasswordService();
        client.Password = passwordService.HashPassword(updatedClient.Password);
      }
      
      client.IdCodPostal = updatedClient.IdCodPostal;

      _context.Clientes.Update(client);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }

[HttpGet]
public IActionResult Delete(int id)
{
    var client = _context.Clientes.FirstOrDefault(c => c.IdClientes == id); 
    if (client == null)
    {
        return NotFound(); 
    }
    return View(client); 
}
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int id)
{
    var client = _context.Clientes.FirstOrDefault(c => c.IdClientes == id);
    if (client == null)
    {
        return NotFound();
    }

    _context.Clientes.Remove(client);
    _context.SaveChanges();

    return RedirectToAction("Index");
}


  }
}