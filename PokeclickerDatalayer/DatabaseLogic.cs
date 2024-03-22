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

    public static List<IDictionary<string, object>> ExecuteQuery(string query, bool isUpdate = false)
    {
        List<IDictionary<string, object>> results = new List<IDictionary<string, object>>();
        MySqlCommand command = new MySqlCommand(query, connection);
        connection.Open();
        
        MySqlDataReader reader = command.ExecuteReader();
        
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

    public static object? ExecuteSql(string sql, object? obj = null, string[]? objectParams = null, bool isUpdate = false)
    {
        MySqlCommand command = new MySqlCommand(sql, connection);
        connection.Open();
        
        if (isUpdate)
        {
            command.ExecuteNonQuery();
            
            return null;
        }
        else
        {
           MySqlDataReader reader = command.ExecuteReader();
           
           return reader;
        }
    }
}