using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySql.Data.MySqlClient;
using PokeclickerDatalayer;
using PokemonClicker.Models;
using PokemonClickerClassLibrary;

namespace PokemonClicker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    public MySqlConnection connection;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        connection = new MySqlConnection(connectionString);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!HttpContext.Session.TryGetValue("user", out _))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Login" },  
                { "action", "Index" }  
  
            });  
        }
        
        base.OnActionExecuting(context);
    }

    public IActionResult Index()
    {
        
        return View();
    }

    public IActionResult SaveData(int points)
    {
        DatabaseLogic.ExecuteQuery($"UPDATE player set points = {points} WHERE player_id = 1");

        TempData["success"] = "Successfully saved";
        
        return RedirectToAction("Game");
    }

    public IActionResult Game()
    {
        ViewData["success"] = TempData["success"];
        connection.Close();

        
        var GameViewModel = new GameViewModel();
        GameViewModel.Player = Player.GetPlayer("mryusuf");
        GameViewModel.Pokemons = Pokemon.GetPokemons();
        
        return View(GameViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}