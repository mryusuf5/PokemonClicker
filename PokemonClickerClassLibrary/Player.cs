namespace PokemonClickerClassLibrary;

public interface IPlayer
{
    public List<Player>? GetPlayer(string loginusername);
    public Player MapToPlayer(IDictionary<string, object> result);
    public void RegisterPlayer(string username, string password);

    public void UpdatePlayerPoints(int points, int? playerId);

    public List<Player>? GetPlayers();
};

public class Player: IPlayer
{
    private readonly IPlayer _playerInterface;
    
    public int? _points;
    public int? _playerId;
    public string _username;
    public string _password;
    public int? _isAdmin;
    

    public Player(string username, int? points, int? isAdmin, string password, int? playerId)
    {
        _isAdmin = isAdmin;
        _points = points;
        _username = username;
        _password = password;
        _playerId = playerId;
    }

    public List<Player>? GetPlayer(string loginusername)
    {
        return _playerInterface.GetPlayer(loginusername);
    }

    public Player MapToPlayer(IDictionary<string, object> result)
    {
        return new Player("asd", 12, 1, "asd", 1);
    }

    public void RegisterPlayer(string username, string password)
    {
        
    }

    public void UpdatePlayerPoints(int points, int? playerId)
    {
        
    }

    public List<Player> GetPlayers()
    {
        return _playerInterface.GetPlayers();
    }
}