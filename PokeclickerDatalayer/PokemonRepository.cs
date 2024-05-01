using PokemonClickerClassLibrary;

namespace PokeclickerDatalayer;

public class PokemonRepository: IPokemon
{
    public List<Pokemon> GetPokemons(int? playerId)
    {
        var pokemonData = DatabaseLogic.ExecuteQuery($"SELECT * FROM pokemons WHERE player_id = {playerId}", MapToPokemon);

        return pokemonData;
    }

    public void CatchPokemon(string pokemonName, string pokemonImage, int? level, int? playerId, int? catchRate)
    {
        DatabaseLogic.InsertUpdateQuery($"INSERT INTO pokemons(name, level, image, player_id, catch_rate) VALUES('{pokemonName}', '{level}', '{pokemonImage}', '{playerId}', '{catchRate}')");
    }
    
    public Pokemon MapToPokemon(IDictionary<string, object> result)
    {
        string name = result["name"].ToString();
        string image = result["image"].ToString();
        int? level = result["level"] as int?;
        int? playerId = result["player_id"] as int?;
        int? catchRate = result["catch_rate"] as int?;

        return new Pokemon(name, image, level, playerId, catchRate);   
    }
}