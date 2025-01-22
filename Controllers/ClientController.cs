using Microsoft.AspNetCore.Mvc;
using mini_project_csharp.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace mini_project_csharp.Controllers
{
  [Authorize]
  public class ClientController : Controller
  {
    public IActionResult Index()
    {
      return View();
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
