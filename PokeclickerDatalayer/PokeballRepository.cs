using PokemonClickerClassLibrary;

namespace PokeclickerDatalayer;

public class PokeballRepository: IPokeball
{
    public List<Pokeball> GetPokeballs()
    {
        return DatabaseLogic.ExecuteQuery("SELECT * FROM pokeballs", MapToPokeball);
    }

    public List<Pokeball> GetPokeball(int ballId)
    {
        return DatabaseLogic.ExecuteQuery($"SELECT * FROM pokeballs WHERE id = {ballId}", MapToPokeball);
    }
    
    public List<Pokeball> GetPokeballsLimit()
    {
        return DatabaseLogic.ExecuteQuery("SELECT * FROM pokeballs ORDER BY `pokeballs`.`id` DESC LIMIT 5", MapToPokeball);
    }
    
    public void InsertPokeball(string name, int? price, string image, int? catchRate)
    {
        DatabaseLogic.InsertUpdateQuery($"INSERT INTO pokeballs(name, price, image, catch_rate) VALUES('{name}', '{price}', '{image}', '{catchRate}')");
    }
    
    public void DeletePokeball(int? id)
    {
        DatabaseLogic.InsertUpdateQuery($"DELETE FROM pokeballs WHERE id = '{id}'");
    }

    public void BuyPokeball(int? playerId, int? ballId)
    {
        DatabaseLogic.InsertUpdateQuery($"INSERT INTO ball_inventory(player_id, ball_id) VALUES('{playerId}', '{ballId}')");
    }

    public void UsePokeball(int? playerId, int? ballId)
    {
        DatabaseLogic.InsertUpdateQuery($"DELETE FROM ball_inventory WHERE player_id = '{playerId}' AND ball_id = '{ballId}' LIMIT 1");
    }
    
    public List<Pokeball> GetPlayerPokeballs(int? playerId)
    {
        return DatabaseLogic.ExecuteQuery($"SELECT pokeballs.id, name, price, image, catch_rate FROM ball_inventory INNER JOIN pokeballs ON ball_inventory.ball_id = pokeballs.id WHERE player_id = '{playerId}';", MapToPokeball);
    }
    
    public Pokeball MapToPokeball(IDictionary<string, object> result)
    {
        string name = result["name"].ToString();
        int? price = result["price"] as int?;
        string image = result["image"].ToString();
        int? id = result["id"] as int?;
        int? catchRate = result["catch_rate"] as int?;

        return new Pokeball(name, price, image, id, catchRate);
    }
}