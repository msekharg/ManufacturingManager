using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ManufacturingManager.ADO
{
    public static class DatabaseFactory
    {
        public static string GetConnectionString(string name)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            var directory = Directory.GetCurrentDirectory();

            builder.AddJsonFile(Path.Combine(directory, "appsettings.json"));

            var root = builder.Build();

            var connectionString = root.GetConnectionString(name);

            return connectionString;
        }


        public static Database CreateDatabase(string name)
        {
            string connection = GetConnectionString(name);

            var newconString = new SqlConnection(connection);// ConnectionStringSettings();

            newconString.ConnectionString = connection;
            return new SqlDatabase(newconString);
        }

        public static string GetDbConnString(string name)
        {
            string connection = GetConnectionString(name);
            return connection;
        }
    }
}
