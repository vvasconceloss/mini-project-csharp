using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Data;
using mini_project_csharp.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult Add()
    {
      return View();
    }
    
  }
}
