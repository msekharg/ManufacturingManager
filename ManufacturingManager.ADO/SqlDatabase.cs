using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Security.Permissions;
using System.Xml;
using Microsoft.Data.SqlClient;

namespace ManufacturingManager.ADO
{
    /// <summary>
    /// Derived class that is for SQL Server use.  Extension methods are created for ease of use.
    /// </summary>

    public class SqlDatabase : Database
    {
        //public SqlDatabase(System.Configuration.ConnectionStringSettings connectionString)
        public SqlDatabase(SqlConnection connectionString)
            : base(connectionString, SqlClientFactory.Instance)
        {
        }

        private string _ParamToken = "@";

        public override string ParameterToken
        {
            get => _ParamToken;
            set { }
        }

     
        public XmlReader ExecuteXmlReader(DbCommand command)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            DbConnection connection = OpenConnection();
            PrepareCommand(command, connection);
            return DoExecuteXmlReader(sqlCommand);
        }

        public XmlReader ExecuteXmlReader(DbCommand command, DbTransaction transaction)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            PrepareCommand(sqlCommand, transaction);
            return DoExecuteXmlReader(sqlCommand);
        }

        private XmlReader DoExecuteXmlReader(SqlCommand sqlCommand)
        {
            try
            {
                XmlReader reader = sqlCommand.ExecuteXmlReader();
                return reader;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        }

        private static SqlCommand CheckIfSqlCommand(DbCommand command)
        {
            SqlCommand sqlCommand = command as SqlCommand;
            if (sqlCommand == null) throw new ArgumentException("The command must be a SqlCommand.", "command");
            return sqlCommand;
        }

        private void OnSqlRowUpdated(object sender, SqlRowUpdatedEventArgs rowThatCouldNotBeWritten)
        {
            if (rowThatCouldNotBeWritten.RecordsAffected == 0)
            {
                if (rowThatCouldNotBeWritten.Errors != null)
                {
                    rowThatCouldNotBeWritten.Row.RowError = "Failed to update row ";
                    rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }

        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)discoveryCommand);
        }

        protected override int UserParametersStartIndex()
        {
            return 1;
        }

        public override string BuildParameterName(string name)
        {
            if (name.StartsWith(this.ParameterToken) == false)
            {
                return this.ParameterToken + name;
            }
            return name;
        }

        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((SqlDataAdapter)adapter).RowUpdated += OnSqlRowUpdated;
        }

        protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            int returnParameterCount = 1;
            int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        public virtual void AddParameter(DbCommand command, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        public void AddParameter(DbCommand command, string name, SqlDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        public void AddOutParameter(DbCommand command, string name, SqlDbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        public void AddInParameter(DbCommand command, string name, SqlDbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        public void AddInParameter(DbCommand command, string name, SqlDbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        public void AddInParameter(DbCommand command, string name, SqlDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        protected DbParameter CreateParameter(string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            SqlParameter param = CreateParameter(name) as SqlParameter;
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        protected virtual void ConfigureParameter(SqlParameter param, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.SqlDbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }
    }
}
