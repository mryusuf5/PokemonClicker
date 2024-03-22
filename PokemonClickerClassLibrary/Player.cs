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
}