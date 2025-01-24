using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Data;
using mini_project_csharp.Models;
using mini_project_csharp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace mini_project_csharp.Controllers
{
  [Authorize] // Esta anotação garante que apenas utilizadores autenticados possam acessar este controlador
  public class ClientController : Controller
  {
    private readonly ApplicationDbContext _context;

    // Construtor que inicializa o contexto da base de dados
    public ClientController(ApplicationDbContext context)
    {
      _context = context;
    }
    
    public IActionResult Index(int pageNumber = 1, int pageSize = 5)
    {
      var clients = _context.Clientes.Include(c => c.CodPostal).ToList();
      int totalCount = clients.Count;
      int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
      
      var items = clients.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
      
      var pagedResult = new PagedResult<Client>
      {
        Items = items,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalPages = totalPages,
        TotalCount = totalCount
      };
      
      ViewBag.LoggedInUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
      
      return View(pagedResult);
    }

    [HttpGet] // Abre a pagina para adicionar um novo cliente
    public IActionResult Add()
    {
      var codPostais = _context.CodPostals.Select(c => new SelectListItem
      {
        Value = c.IdCodPostal.ToString(),
        Text = c.Codpostal + " - " + c.Localidade // Mostra o codigo postal formatado
      }).ToList();

      ViewBag.CodPostais = codPostais; // Passa os codigos postais para o formulário
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Ajuda a prevenir ataques CSRF
    public IActionResult Add(Client newClient)
    {
      if (!ModelState.IsValid) // Verifica se os dados enviados são válidos
      {
        var codPostais = _context.CodPostals.Select(c => new SelectListItem
        {
          Value = c.IdCodPostal.ToString(),
          Text = c.CodPostalFormatado
        }).ToList();

        ViewBag.CodPostais = codPostais;
        return View(newClient); // Se inválido, volta para o formulário com os dados
      }

      var passwordService = new PasswordService();
      newClient.Password = passwordService.HashPassword(newClient.Password); // Encripta a palavra-passe

      // Adiciona o novo cliente ao banco de dados
      _context.Clientes.Add(newClient);
      _context.SaveChanges();

      return RedirectToAction("Index"); // Redireciona para a página inicial
    }

    [HttpGet] // Abre a página para editar um cliente
    public IActionResult Edit(int id)
    {
      var client = _context.Clientes.Find(id); // Procura o cliente pelo ID

      if (client == null) // Se não encontrar o cliente, retorna um erro 404
      {
        return NotFound();
      }

      var codPostais = _context.CodPostals.Select(c => new SelectListItem
      {
        Value = c.IdCodPostal.ToString(),
        Text = c.CodPostalFormatado
      }).ToList();

      ViewBag.CodPostais = codPostais; // Passa os códigos postais para o formulário
      return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Client updatedClient)
    {
      if (!ModelState.IsValid) // Verifica se os dados são válidos
      {
        var codPostais = _context.CodPostals.Select(c => new SelectListItem
        {
          Value = c.IdCodPostal.ToString(),
          Text = c.CodPostalFormatado
        }).ToList();

        ViewBag.CodPostais = codPostais;
        return View(updatedClient);
      }

      var client = _context.Clientes.Find(updatedClient.IdClientes); // Procura o cliente a ser atualizado

      if (client == null)
      {
        return NotFound();
      }

      // Atualiza os dados do cliente
      client.Nome = updatedClient.Nome;
      client.Apelido = updatedClient.Apelido;
      client.Endereco = updatedClient.Endereco;
      client.Telefone = updatedClient.Telefone;
      client.Nif = updatedClient.Nif;
      client.Email = updatedClient.Email;

      if (!string.IsNullOrEmpty(updatedClient.Password)) // Atualiza a palavra-passe, se fornecida
      {
        var passwordService = new PasswordService();
        client.Password = passwordService.HashPassword(updatedClient.Password);
      }

      client.IdCodPostal = updatedClient.IdCodPostal;

      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (client.IdClientes.ToString() == userId)
      {
        var claims = new List<Claim>
        {
          new(ClaimTypes.Name, updatedClient.Nome),
          new(ClaimTypes.Email, updatedClient.Email),
          new(ClaimTypes.NameIdentifier, client.IdClientes.ToString())
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)).Wait();
      }

      _context.Clientes.Update(client);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpGet] // Abre a página para confirmar a exclusão de um cliente
    public IActionResult Delete(int id)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var client = _context.Clientes.FirstOrDefault(c => c.IdClientes == id);
      
      if (client == null)
      {
        return NotFound();
      }
      
      if (client.IdClientes.ToString() == userId)
      {
        ModelState.AddModelError(string.Empty, "Você não pode apagar um cliente que está loggado.");
        return RedirectToAction("Index");
      }
      
      return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Client client)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var clientToDelete = _context.Clientes.FirstOrDefault(c => c.IdClientes == client.IdClientes);

      if (clientToDelete == null)
      {
        return NotFound();
      }

      if (clientToDelete.IdClientes.ToString() == userId)
      {
        ModelState.AddModelError(string.Empty, "Você não pode apagar um cliente que está loggado.");
        return RedirectToAction("Index");
      }
      
      _context.Clientes.Remove(clientToDelete); 
      _context.SaveChanges();

      return RedirectToAction("Index"); // Volta para a página inicial
    }
  }
}
