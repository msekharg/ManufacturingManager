using Microsoft.Data.SqlClient;

namespace ManufacturingManager.ADO
{
    public class ConnectionString
    {
        private readonly string _connectionString;

        //public ConnectionString(System.Configuration.ConnectionStringSettings connectionString)
        public ConnectionString(SqlConnection connectionString)
        {
            if (string.IsNullOrEmpty(connectionString.ConnectionString)) throw new ArgumentException("The value can not be null or an empty string.", nameof(connectionString));
            this._connectionString = connectionString.ConnectionString;
        }

        public override string ToString()
        {
            return _connectionString;
        }
    }
}
