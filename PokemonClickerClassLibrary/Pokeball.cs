namespace PokemonClickerClassLibrary;

public interface IPokeball
{
    public List<Pokeball> GetPokeballs();
    public List<Pokeball> GetPokeballsLimit();
    public void InsertPokeball(string name, int? price, string image, int? catchRate);
    public void DeletePokeball(int? id);
    public void BuyPokeball(int? playerId, int? ballId);
    public List<Pokeball> GetPokeball(int ballId);
    public List<Pokeball> GetPlayerPokeballs(int? playerId);
    public void UsePokeball(int? playerId, int? ballId);
};

public class Pokeball
{
    public string _name;
    public int? _price;
    public string _image;
    public int? _id;
    public int? _catchRate;

    public Pokeball(string name, int? price, string image, int? id, int? catchRate)
    {
        _image = image;
        _name = name;
        _price = price;
        _id = id;
        _catchRate = catchRate;
    }
}