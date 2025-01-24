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
  [Authorize]
  public class ClientController : Controller
  {
    private readonly ApplicationDbContext _context;
    
    public ClientController(ApplicationDbContext context)
    {
      _context = context;
    }
    
    //MÉTODO PARA RETORNAR A VIEW DE CLIENTES
    public IActionResult Index(int pageNumber = 1, int pageSize = 5)
    {
      var clients = _context.Clientes.Include(c => c.CodPostal).ToList();
      int totalCount = clients.Count; //VARIÁVEL RESPONSÁVEL POR DEFINIR O TOTAL DE CLIENTES EXISTENTES

      /*
        VARIÁVEL QUE RECEBE O TOTAL DE "PÁGINAS" (PAGINATION) COM BASE
        NA QUANTIDADE DE CLIENTES QUE EXISTEM (5 POR PÁGINA APENAS)
      */
      int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
      
      //VARIÁVEL RESPONSÁVEL POR MUDAR OS CLIENTES APÓS TROCARMOS A PÁGINA
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

    //MÉTOD RESPONSÁVEL POR ABRIR A VIEW PARA ADICIONAR CLIENTES
    [HttpGet]
    public IActionResult Add()
    {
      //VARIÁVEL QUE PREENCHE O SELECT DA VIEW COM OS CÓDIGOS POSTAIS FORMATADOS
      var codPostais = _context.CodPostals.Select(c => new SelectListItem
      {
        Value = c.IdCodPostal.ToString(),
        Text = c.CodPostalFormatado
      }).ToList();

      ViewBag.CodPostais = codPostais;

      return View();
    }

    //MÉTODO PARA ADICIONAR UM CLIENTE NA BASE DE DADOS
    [HttpPost]
    [ValidateAntiForgeryToken] //AJUDA A PREVINIR ATAQUES CRFS
    public IActionResult Add(Client newClient)
    {
      //VERIFICA SE OS DADOS RECEBIDOS SÃO VÁLIDOS
      if (!ModelState.IsValid)
      {
        var codPostais = _context.CodPostals.Select(c => new SelectListItem
        {
          Value = c.IdCodPostal.ToString(),
          Text = c.CodPostalFormatado
        }).ToList();

        ViewBag.CodPostais = codPostais;

        return View(newClient); //CASO SEJA INVÁLIDO, O UTILIZADOR VOLTA PARA O FORMULÁRIO
      }

      //FAZ A ENCRIPTAÇÃO DA PASSWORD 
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

      //ADICIONA EFETIVAMENTE O CLIENTE A BASE DE DADOS
      _context.Clientes.Add(newClient);
      _context.SaveChanges();

      return RedirectToAction("Index");
    }

    /*
    MÉTODO RESPONSÁVEL POR MOSTRAR O FORMULÁRIO DE EDITAR UM CLIENTE
    COM OS DADOS DO MESMO PREENCHIDOS NOS CAMPOS
    */
    [HttpGet]
    public IActionResult Edit(int id)
    {
      var client = _context.Clientes.Find(id); //PROCURA O CLIENTE UTILIZANDO SEU ID

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

    //MÉTODO PARA EDITAR UM CLIENTE DA BASE DE DADOS
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
      
      //FAZ A ENCRIPTAÇÃO DA PASSWORD SE A MESMA FOR ALTERADA
      if (!string.IsNullOrEmpty(updatedClient.Password))
      {
        var passwordService = new PasswordService();
        client.Password = passwordService.HashPassword(updatedClient.Password);
      }
      
      client.IdCodPostal = updatedClient.IdCodPostal;

      //ATUALIZA OS DADOS EXISTENTES NA CLAIM DO UTILIZADO (RELACIONADO AO SISTEMA DE AUTENTICAÇÃO)
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
    
    //MÉTODO QUE ABRE O FORMULÁRIO DE EXCLUSÃO DE UM CLIENTE
    [HttpGet]
    public IActionResult Delete(int id)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var client = _context.Clientes.FirstOrDefault(c => c.IdClientes == id); //PROCURA O CLIENTE NA BASE DE DADOS
      
      if (client == null)
      {
        return NotFound();
      }
      
      //VERIFICA SE O CLIENTE COM SESSÃO INICIADA ESTÁ TENTANDO APAGAR SEU PRÓPRIO REGISTO
      if (client.IdClientes.ToString() == userId)
      {
        ModelState.AddModelError(string.Empty, "Você não pode apagar um cliente que está loggado.");
        return RedirectToAction("Index");
      }
      
      return View(client);
    }

    //MÉTODO QUE DE FACTO EXECUTA A EXCLUSÃO DO CLIENTE
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
      return RedirectToAction("Index");
    }
  }
}