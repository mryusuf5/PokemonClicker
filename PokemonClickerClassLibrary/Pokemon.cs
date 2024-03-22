namespace PokemonClickerClassLibrary;

public class Pokemon
{
    private string _name;
    public string name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    private int _level;

    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }
    }
}