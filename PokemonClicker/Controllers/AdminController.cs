using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeclickerDatalayer;
using PokemonClicker.Models;
using PokemonClickerClassLibrary;

namespace PokemonClicker.Controllers;

public class AdminController : Controller
{
    private readonly IPokeball _pokeball;
    private readonly IPlayer _player;

    public AdminController()
    {
        _pokeball = new PokeballRepository();
        _player = new PlayerRepository();
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (HttpContext.Session.GetInt32("admin") != 1)
        {
            TempData["error"] = "No permission";
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Login" },  
                { "action", "Index" }  
            }); 
        }
        
        ViewData["success"] = TempData["success"];
        ViewData["error"] = TempData["error"];
        
        base.OnActionExecuting(context);
    }

    // GET
    public IActionResult Index()
    {
        var adminViewModel = new AdminViewModel();
        
        adminViewModel.Pokeballs = _pokeball.GetPokeballsLimit();
        adminViewModel.Players = _player.GetPlayers();
        
        return View(adminViewModel);
    }

    public IActionResult CreatePokeball(string name, int? price, IFormFile image, int? catchRate)
    {
        if (image == null || image.Length <= 0)
        {
            TempData["error"] = "Please select an image";
            return RedirectToAction("index", "Admin");
        }

        if (name == null || name == "")
        {
            TempData["error"] = "Please select a name";
            return RedirectToAction("index", "Admin");
        }
        
        if (price == null)
        {
            TempData["error"] = "Please select a price";
            return RedirectToAction("index", "Admin");
        }

        var fileExtension = Path.GetExtension(image.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "pokeballs");

        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        var filePath = Path.Combine(uploadDir, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(fileStream);
        }

        _pokeball.InsertPokeball(name, price, fileName, catchRate);
        TempData["success"] = "Pokeball added";
        
        return RedirectToAction("index", "Admin");
    }

    public IActionResult Pokeballs()
    {
        var adminViewModel = new AdminViewModel();
        var pokeballs = _pokeball.GetPokeballs();
        
        adminViewModel.Pokeballs = pokeballs;
        
        return View(adminViewModel);
    }

    public IActionResult DeletePokeball(int? id)
    {
        _pokeball.DeletePokeball(id);

        TempData["success"] = "Pokeball deleted";
        
        return RedirectToAction("Pokeballs", "Admin");
    }

    public IActionResult Users()
    {
        return View();
    }
}