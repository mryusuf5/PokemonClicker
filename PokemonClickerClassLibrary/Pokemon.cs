namespace PokemonClickerClassLibrary;

public interface IPokemon
{
    public List<Pokemon> GetPokemons(int? playerId);
    public Pokemon MapToPokemon(IDictionary<string, object> result);
    public void CatchPokemon(string pokemonName, string pokemonImage, int? level, int? playerId, int? catchRate);
}

public class Pokemon
{
    public string _name;
    public int? _level;
    public string _image;
    public int? _playerId;
    public int? _catchRate;

    public Pokemon(string name, string image, int? level, int? playerId, int? catchRate)
    {
        _name = name;
        _image = image;
        _level = level;
        _playerId = playerId;
        _catchRate = catchRate;
    }
}