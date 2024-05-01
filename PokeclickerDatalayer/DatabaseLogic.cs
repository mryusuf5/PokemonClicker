using System.Collections;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PokemonClickerClassLibrary;
using static PokemonClickerClassLibrary.Player;

namespace PokeclickerDatalayer;

public interface IMapper<T>
{
    T Map(IDictionary<string, object> result);
}

public class DatabaseLogic
{
    private static IConfiguration configuration;
    public static MySqlConnection connection;

    static DatabaseLogic()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
        
        configuration = builder.Build();
        
        connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public static List<T>? ExecuteQuery<T>(string query, Func<IDictionary<string, object>, T>? mapper)
    {
        List<IDictionary<string, object>> results = new List<IDictionary<string, object>>();
        MySqlCommand command = new MySqlCommand(query, connection);
        
        connection.Open();
        
        MySqlDataReader reader = null;
        try
        {
            reader = command.ExecuteReader();
        }
        catch (Exception e)
        {
            command.ExecuteNonQuery();
            connection.Close(); 
            return null;
        }
        
        while (reader.Read())
        {
            var result = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                result.Add(reader.GetName(i), reader.GetValue(i));
            }
            results.Add(result);
        }   

        connection.Close();

        List<T> mappedResults = new List<T>();
        foreach (var result in results)
        {
            T mappedResult = mapper(result);
            mappedResults.Add(mappedResult);
        }
        
        return mappedResults;
    }

    public static void InsertUpdateQuery(string query)
    {
        MySqlCommand command = new MySqlCommand(query, connection);
        connection.Open();

        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        connection.Close();
    }
}