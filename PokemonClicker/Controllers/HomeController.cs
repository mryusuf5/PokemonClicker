using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeclickerDatalayer;
using PokemonClicker.Models;
using PokemonClickerClassLibrary;
using Newtonsoft.Json;

namespace PokemonClicker.Controllers;

public class HomeController : Controller
{
    private readonly IPlayer _player;
    private readonly IPokemon _pokemon;
    private readonly IPokeball _pokeball;
    public List<Player> player;
    public HomeController()
    {
        _player = new PlayerRepository();
        _pokemon = new PokemonRepository();
        _pokeball = new PokeballRepository();
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

        if (HttpContext.Session.TryGetValue("user", out _))
        {
            HttpContext.Session.TryGetValue("user", out var user);

            var userJsonString = Encoding.UTF8.GetString(user);

            var parsedUser = JsonConvert.DeserializeObject<Player>(userJsonString);
        
            player = _player.GetPlayer(parsedUser._username);
        }
        
        ViewData["success"] = TempData["success"];
        ViewData["error"] = TempData["error"];
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SaveData(int points, int? playerId, int fromAdminPanel = 0)
    {
        if (fromAdminPanel == 0)
        {
            _player.UpdatePlayerPoints(points, player[0]._playerId);

            TempData["success"] = "Successfully saved";
            return RedirectToAction("Game");
        }
        else
        {
            _player.UpdatePlayerPoints(points, playerId);
            
            return RedirectToAction("Index", "Admin");
        }
    }

    public IActionResult Game()
    {
        HttpContext.Session.TryGetValue("user", out var user);

        var userJsonString = Encoding.UTF8.GetString(user);

        var parsedUser = JsonConvert.DeserializeObject<Player>(userJsonString);
        
        var players = _player.GetPlayer(parsedUser._username);
        
        var GameViewModel = new GameViewModel();
        GameViewModel.Player = players[0];
        GameViewModel.Pokemons = _pokemon.GetPokemons(this.player[0]._playerId);
        GameViewModel.Pokeballs = _pokeball.GetPokeballs();
        GameViewModel.PlayerPokeballs = _pokeball.GetPlayerPokeballs(this.player[0]._playerId);
        
        return View(GameViewModel);
    }

    public IActionResult BuyPokeball(int ballId)
    {
        var player = _player.GetPlayer(this.player[0]._username);
        var pokeball = _pokeball.GetPokeball(ballId);

        if (pokeball[0]._price > player[0]._points)
        {
            TempData["error"] = "You dont have enough points";
            return RedirectToAction("Game");
        }

        int newpoints = (int)player[0]._points - (int)pokeball[0]._price;
        
        _player.UpdatePlayerPoints(newpoints, this.player[0]._playerId);
        _pokeball.BuyPokeball(player[0]._playerId, ballId);
        
        TempData["success"] = "Pokeball bought";
        return RedirectToAction("Game");
    }

    public IActionResult CatchPokemon(string pokemonName, string pokemonImage, int battlePokemonStats, int pokeballCatchRate, string pokeballName, int? ballId, int? pokemonCatchRate, int? level)
    {
        _pokeball.UsePokeball(this.player[0]._playerId, ballId);
        
        if (pokeballCatchRate == 100)
        {
            _pokemon.CatchPokemon(pokemonName, pokemonImage, level, this.player[0]._playerId, pokemonCatchRate);
            TempData["success"] = "Congrats on catching " + pokemonName + "!";
            
            return RedirectToAction("Game");
        }

        double catchRate = Convert.ToDouble(pokemonCatchRate);

        double pokeballCatchRateCalc = Convert.ToDouble(pokeballCatchRate) / 255;

        catchRate *= pokeballCatchRateCalc;

        catchRate += Convert.ToDouble(pokemonCatchRate);
        
        Random random = new Random();
        int randomCatchValue = random.Next(0, 256);
        
        if (randomCatchValue < catchRate)
        {
            _pokemon.CatchPokemon(pokemonName, pokemonImage, level, this.player[0]._playerId, pokemonCatchRate);
            TempData["success"] = "Congrats on catching " + pokemonName + "!";
        }
        else
        {
            TempData["error"] = "Too bad you didnt manage to catch " + pokemonName + " :(";
        }
        
        return RedirectToAction("Game");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}