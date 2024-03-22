using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        Player player = new Player();
        var data = DatabaseLogic.ExecuteQuery("SELECT * FROM player WHERE id = 1");

        data[0].TryGetValue("username", out object username);
        player.username = username as string;
        
        ViewData["success"] = TempData["success"];
        connection.Close();
        
        return View(player);
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult LoginPost(string username, string password)
    {
        Player player = new Player();
        MySqlCommand command = new MySqlCommand("SELECT * FROM player WHERE player_id = 1", connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            player.username = reader["username"].ToString();
            player.password = reader["password"].ToString();
        }
        
        return RedirectToAction("Game");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}