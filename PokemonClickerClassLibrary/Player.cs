using PokeclickerDatalayer;

namespace PokemonClickerClassLibrary;

public class Player
{
    private int _points;
    public int playerId = 1;
    public string username;
    public string password;

    public int points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
        }
    }

    private float _multiplier;

    public float multiplier
    {
        get
        {
            return _multiplier;
        }
        set
        {
            _multiplier = value;
        }
    }

    public static Player? GetPlayer(string loginusername)
    {
        Player player = new Player();
        
        var data = DatabaseLogic.ExecuteQuery($"SELECT * FROM player WHERE username = '{loginusername}'");

        if (data == null)
        {
            return null;
        }

        data[0].TryGetValue("username", out object name);
        player.username = name as string;
        
        data[0].TryGetValue("points", out object points);
        player.points = (int)points;
        
        data[0].TryGetValue("password", out object password);
        player.password = password as string;

        return player;
    }
}