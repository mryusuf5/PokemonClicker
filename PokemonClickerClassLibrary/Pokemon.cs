using PokeclickerDatalayer;

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

    public string image;

    public Pokemon(string name, string image)
    {
        this.name = name;
        this.image = image;
    }

    public static List<Pokemon> GetPokemons()
    {
        var pokemonData = DatabaseLogic.ExecuteQuery($"SELECT * FROM pokemons");
        List<Pokemon> pokemons = new List<Pokemon>();
        
        foreach (var pokemon in pokemonData)
        {
            pokemon.TryGetValue("name", out object name);
            pokemon.TryGetValue("image", out object image);
            pokemons.Add(new Pokemon(name as string, image as string));
        }

        return pokemons;
    }
}