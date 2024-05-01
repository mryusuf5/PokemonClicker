using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PokemonClickerClassLibrary;

namespace PokeclickerDatalayer;

public class PlayerRepository: IPlayer
{
    private static IConfiguration configuration;
    public static MySqlConnection connection;

    static PlayerRepository()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
        
        configuration = builder.Build();
        
        connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
    
    public List<Player>? GetPlayer(string loginusername)
    {
        return DatabaseLogic.ExecuteQuery($"SELECT * FROM player WHERE username = '{loginusername}'", MapToPlayer);
    }
    
    public void RegisterPlayer(string username, string password)
    {
        DatabaseLogic.InsertUpdateQuery($"INSERT INTO player(username, password, is_admin, points) VALUES('{username}', '{password}', '0', '1000')");
    }
    
    public void UpdatePlayerPoints(int points, int? playerId)
    {
        DatabaseLogic.InsertUpdateQuery($"UPDATE player set points = '{points}' WHERE id = '{playerId}'");
    }

    public List<Player> GetPlayers()
    {
        return DatabaseLogic.ExecuteQuery($"SELECT * FROM player", MapToPlayer);
    }
    
    public Player MapToPlayer(IDictionary<string, object> result)
    {
        string username = result["username"].ToString();
        int? points = result["points"] as int?;
        string password = result["password"].ToString();
        int? isAdmin = result["is_admin"] as int?;
        int? playerId = result["id"] as int?;

        return new Player(username, points, isAdmin, password, playerId);
    }
}