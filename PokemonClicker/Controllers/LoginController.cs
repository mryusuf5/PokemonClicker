using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeclickerDatalayer;
using PokemonClicker.Models;
using PokemonClickerClassLibrary;

namespace PokemonClicker.Controllers;

public class LoginController : Controller
{

    private readonly PlayerRepository _player;
    
    public LoginController()
    {
        _player = new PlayerRepository();
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.RouteData.Values["Action"] as string == "Logout") return;
        if (HttpContext.Session.TryGetValue("user", out _))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },  
                { "action", "Game" }  
  
            });  
        }
        
        base.OnActionExecuting(context);
        
        ViewData["success"] = TempData["success"];
        ViewData["error"] = TempData["error"];
    }
    
    
    public IActionResult Index()
    {
        ViewData["error"] = TempData["error"];
        
        return View();
    }
    
    public IActionResult LoginPost(string username, string password)
    {
        var player = _player.GetPlayer(username);

        if (player.Count <= 0)
        {
            TempData["error"] = "Username not found";
            
            return RedirectToAction("Index");
        }
        
        if (player[0]._password != password)
        {
            TempData["error"] = "Wrong password";
            
            return RedirectToAction("Index");
        }
        else
        {
            if (player[0]._isAdmin == 1)
            {
                string user = Newtonsoft.Json.JsonConvert.SerializeObject(player[0]);
                
                HttpContext.Session.SetString("user", user);
                HttpContext.Session.SetInt32("admin", 1);
            }
            else
            {
                string user = Newtonsoft.Json.JsonConvert.SerializeObject(player[0]);
                HttpContext.Session.SetString("user", user);
                HttpContext.Session.SetInt32("admin", 0);
            }
            
            Console.WriteLine(HttpContext.Session.TryGetValue("user", out _));
        }

        return RedirectToAction("Game", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult RegisterPost(string username, string password, string repeatPassword)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repeatPassword))
        {
            TempData["error"] = "Please fill in all inputs";
            return RedirectToAction("Register");
        }

        if (password.Length < 6)
        {
            TempData["error"] = "Password should have atleast 6 characters";
            return RedirectToAction("Register");
        }

        if (password != repeatPassword)
        {
            TempData["error"] = "Passwords dont match";
            return RedirectToAction("Register");
        }

        var player = _player.GetPlayer(username);
        
        if (player.Count == 1)
        {
            TempData["error"] = "Username already taken";
            return RedirectToAction("Register");
        }
        
        TempData["success"] = "Successfully registered";
        _player.RegisterPlayer(username, password);
        
        return RedirectToAction("Index");
    }
    
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("user");
        return RedirectToAction("Index");
    }
}