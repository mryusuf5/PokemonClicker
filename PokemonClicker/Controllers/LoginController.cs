using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonClicker.Models;
using PokemonClickerClassLibrary;

namespace PokemonClicker.Controllers;

public class LoginController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.RouteData.Values["Action"] as string == "Logout") return;
        if (HttpContext.Session.TryGetValue("user", out _))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },  
                { "action", "Index" }  
  
            });  
        }
        
        base.OnActionExecuting(context);
    }
    
    
    public IActionResult Index()
    {
        ViewData["error"] = TempData["error"];
        
        return View();
    }
    
    public IActionResult LoginPost(string username, string password)
    {
        Player player = Player.GetPlayer(username);

        if (player == null)
        {
            TempData["error"] = "Username not found";
            
            return RedirectToAction("Index");
        }
        
        if (player.password != password)
        {
            TempData["error"] = "Wrong password";
            
            return RedirectToAction("Index");
        }
        else
        {
            HttpContext.Session.SetString("user", player.username);
        }

        return RedirectToAction("Game", "Home");
    }
    
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("user");
        return RedirectToAction("Index");
    }
}