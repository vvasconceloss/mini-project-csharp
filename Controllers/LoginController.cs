using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return  View();
    }
}
