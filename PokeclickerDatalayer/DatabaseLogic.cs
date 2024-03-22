using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace PokeclickerDatalayer;

public static class DatabaseLogic
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

    public static List<IDictionary<string, object>>? ExecuteQuery(string query)
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
            Console.WriteLine(e);
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
        return results;
    }
}