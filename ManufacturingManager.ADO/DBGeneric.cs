using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;


namespace ManufacturingManager.ADO
{
    public class DbGeneric
    {
        private readonly IConfiguration _configuration;

        public DbGeneric() { }
        public DbGeneric(IConfiguration configuration)
        {
            _configuration = configuration;

        }


        //public string GetAll(string name)
        //{
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
        //        return connectionString;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        //change it to getconnString
        public string GetAllForCore(string name)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            var directorio = Directory.GetCurrentDirectory();
            builder.AddJsonFile(Path.Combine(directorio, "appsettings.json"));

            var root = builder.Build();

            var connectionString = root.GetConnectionString(name);
            //    .AddJsonFile("appsettings.json.config", optional: true)
            //    .Build();
            //string connectionString = _configuration["ConnectionStrings:DefaultConnection"].ToString();
            //string connectionString = configuration["ConnectionStrings:"+name];
            return connectionString;
        }
    }
}
