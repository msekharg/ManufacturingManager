using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Data.SqlClient;

namespace ManufacturingManager.ADO
{
    public abstract class Database
    {
        internal ILog _logger = LogManager.GetLogger(typeof(Database));
        public abstract string ParameterToken { get; set; }

        private readonly ConnectionString _connectionString;
        private readonly DbProviderFactory _dbProviderFactory;

        protected Database(SqlConnection connectionString, DbProviderFactory dbProviderFactory)
        {
            if (string.IsNullOrEmpty(connectionString.ConnectionString)) throw new ArgumentException("The value can not be null or an empty string.", "connectionString");

            _connectionString = new ConnectionString(connectionString);

            _dbProviderFactory = dbProviderFactory ?? throw new ArgumentNullException("dbProviderFactory");
        }

        protected internal string ConnectionString => this._connectionString.ToString();

        public DbProviderFactory DbProviderFactory => this._dbProviderFactory;

        public virtual DateTime GetDbDateTime()
        {
            throw new Exception("GetDBDateTime method in abstract class Database must be overridden.");
        }

        public DbConnection CreateConnection()
        {
            DbConnection newConnection = _dbProviderFactory.CreateConnection();
            if (newConnection != null)
            {
                newConnection.ConnectionString = ConnectionString;

                return newConnection;
            }

            return null;
        }

        public virtual DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException("The value can not be null or an empty string.", "storedProcedureName");

            return CreateCommandByCommandType(CommandType.StoredProcedure, storedProcedureName);
        }

        public virtual DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException("The value can not be null or an empty string.", "storedProcedureName");

            DbCommand command = CreateCommandByCommandType(CommandType.StoredProcedure, storedProcedureName);

            if (SameNumberOfParametersAndValues(command, parameterValues) == false)
            {
                throw new InvalidOperationException("The number of parameters does not match number of values for stored procedure.");
            }

            AssignParameterValues(command, parameterValues);
            return command;
        }

        public DbCommand GetStoredProcCommandWithSourceColumns(string storedProcedureName, params string[] sourceColumns)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException("The value can not be null or an empty string.", "storedProcedureName");
            if (sourceColumns == null) throw new ArgumentNullException("sourceColumns");

            DbCommand dbCommand = GetStoredProcCommand(storedProcedureName);

            //we do not actually set the connection until the Fill or Update, so we need to temporarily do it here so we can set the sourcecolumns
            using (DbConnection connection = CreateConnection())
            {
                dbCommand.Connection = connection;
                DiscoverParameters(dbCommand);
            }

            int iSourceIndex = 0;
            foreach (IDataParameter dbParam in dbCommand.Parameters)
            {
                if ((dbParam.Direction == ParameterDirection.Input) | (dbParam.Direction == ParameterDirection.InputOutput))
                {
                    dbParam.SourceColumn = sourceColumns[iSourceIndex];
                    iSourceIndex++;
                }
            }

            return dbCommand;
        }

        //public DbCommand GetSqlStringCommand(string query)
        //{
        //    if (string.IsNullOrEmpty(query)) throw new ArgumentException("The value can not be null or an empty string.", "query");

        //    return CreateCommandByCommandType(CommandType.Text, query);
        //}


        public DbDataAdapter GetDataAdapter()
        {
            return GetDataAdapter(UpdateBehavior.Standard);
        }

        protected DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior)
        {
            DbDataAdapter adapter = _dbProviderFactory.CreateDataAdapter();

            if (updateBehavior == UpdateBehavior.Continue)
            {
                this.SetUpRowUpdatedEvent(adapter);
            }
            return adapter;
        }

        protected virtual void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
        }

        public virtual void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
        {
            LoadDataSet(command, dataSet, new[] { tableName });
        }

        public virtual void LoadDataSet(DbCommand command, DataSet dataSet, string tableName, DbTransaction transaction)
        {
            LoadDataSet(command, dataSet, new[] { tableName }, transaction);
        }

        public virtual void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            using (DbConnection connection = CreateConnection())
            {
                PrepareCommand(command, connection);
                DoLoadDataSet(command, dataSet, tableNames);
            }
        }

        public virtual void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoLoadDataSet(command, dataSet, tableNames);
        }

        public virtual void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                LoadDataSet(command, dataSet, tableNames);
            }
        }

        public virtual void LoadDataSet(DbTransaction transaction, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                LoadDataSet(command, dataSet, tableNames, transaction);
            }
        }

        public virtual void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                LoadDataSet(command, dataSet, tableNames);
            }
        }

        public void LoadDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                LoadDataSet(command, dataSet, tableNames, transaction);
            }
        }

        public virtual DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table");
            return dataSet;
        }

        public virtual DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table", transaction);
            return dataSet;
        }

        public virtual DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteDataSet(command);
            }
        }

        public virtual DataSet ExecuteDataSet(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteDataSet(command, transaction);
            }
        }

        public virtual DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command);
            }
        }

        public virtual DataSet ExecuteDataSet(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command, transaction);
            }
        }

        public virtual object ExecuteScalar(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            using (DbConnection connection = OpenConnection())
            {
                PrepareCommand(command, connection);
                return DoExecuteScalar(command);
            }
        }

        public virtual object ExecuteScalar(DbCommand command, DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteScalar(command);
        }

        public virtual object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(command);
            }
        }

        public virtual object ExecuteScalar(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(command, transaction);
            }
        }

        public virtual object ExecuteScalar(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteScalar(command);
            }
        }

        public virtual object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteScalar(command, transaction);
            }
        }


        public virtual int ExecuteNonQuery(DbCommand command)
        {
            using (DbConnection connection = OpenConnection())
            {
                PrepareCommand(command, connection);
                return DoExecuteNonQuery(command);
            }
        }

        public virtual int ExecuteNonQuery(DbCommand command, DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteNonQuery(command);
        }

        public virtual int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteNonQuery(command);
            }
        }

        public virtual int ExecuteNonQuery(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteNonQuery(command, transaction);
            }
        }

        public virtual int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteNonQuery(command);
            }
        }


        public virtual int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteNonQuery(command, transaction);
            }
        }

        public virtual IDataReader ExecuteReader(DbCommand command)
        {
            DbConnection connection = OpenConnection();
            PrepareCommand(command, connection);

            try
            {
                return DoExecuteReader(command, CommandBehavior.CloseConnection);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        public virtual IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteReader(command, CommandBehavior.Default);
        }

        public IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteReader(command);
            }
        }

        public IDataReader ExecuteReader(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteReader(command, transaction);
            }
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command);
            }
        }

        public IDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command, transaction);
            }
        }

        public int UpdateDataSet(DataSet dataSet, string tableName,
                                                    DbCommand insertCommand, DbCommand updateCommand,
                                                    DbCommand deleteCommand, UpdateBehavior updateBehavior)
        {
            using (DbConnection connection = OpenConnection())
            {
                if (updateBehavior == UpdateBehavior.Transactional)
                {
                    DbTransaction trans = BeginTransaction(connection);
                    try
                    {
                        int rowsAffected = UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, trans);
                        CommitTransaction(trans);
                        return rowsAffected;
                    }
                    catch
                    {
                        RollbackTransaction(trans);
                        throw;
                    }
                }
                else
                {
                    if (insertCommand != null)
                    {
                        PrepareCommand(insertCommand, connection);
                    }
                    if (updateCommand != null)
                    {
                        PrepareCommand(updateCommand, connection);
                    }
                    if (deleteCommand != null)
                    {
                        PrepareCommand(deleteCommand, connection);
                    }

                    return DoUpdateDataSet(updateBehavior, dataSet, tableName,
                                                  insertCommand, updateCommand, deleteCommand);
                }
            }
        }

        public int UpdateDataSet(DataSet dataSet, string tableName,
                                                    DbCommand insertCommand, DbCommand updateCommand,
                                                    DbCommand deleteCommand, DbTransaction transaction)
        {
            if (insertCommand != null)
            {
                PrepareCommand(insertCommand, transaction);
            }
            if (updateCommand != null)
            {
                PrepareCommand(updateCommand, transaction);
            }
            if (deleteCommand != null)
            {
                PrepareCommand(deleteCommand, transaction);
            }

            return DoUpdateDataSet(UpdateBehavior.Transactional,
                                          dataSet, tableName, insertCommand, updateCommand, deleteCommand);
        }

        public virtual void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }


        public virtual void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            command.Parameters[BuildParameterName(parameterName)].Value = value ?? DBNull.Value;
        }

        public virtual object GetParameterValue(DbCommand command, string name)
        {
            return command.Parameters[BuildParameterName(name)].Value;
        }

        protected static void PrepareCommand(DbCommand command, DbConnection connection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (connection == null) throw new ArgumentNullException("connection");

            command.Connection = connection;
        }

        protected static void PrepareCommand(DbCommand command, DbTransaction transaction)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (transaction == null) throw new ArgumentNullException("transaction");

            PrepareCommand(command, transaction.Connection);
            command.Transaction = transaction;
        }

        protected DbConnection OpenConnection()
        {
            DbConnection connection = null;
            try
            {
                connection = CreateConnection();
                connection.Open();
            }
            catch
            {
                if (connection != null)
                    connection.Close();

                throw;
            }

            return connection;
        }

        protected virtual bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            int numberOfParametersToStoredProcedure = command.Parameters.Count;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        public virtual string BuildParameterName(string name)
        {
            return name;
        }

        public void DiscoverParameters(DbCommand command)
        {
            using (DbConnection discoveryConnection = OpenConnection())
            {
                using (DbCommand discoveryCommand = CreateCommandByCommandType(command.CommandType, command.CommandText))
                {
                    discoveryCommand.Connection = discoveryConnection;
                    DeriveParameters(discoveryCommand);

                    foreach (IDataParameter parameter in discoveryCommand.Parameters)
                    {
                        IDataParameter cloneParameter = (IDataParameter)((ICloneable)parameter).Clone();
                        command.Parameters.Add(cloneParameter);
                    }
                }
            }
        }

        protected abstract void DeriveParameters(DbCommand discoveryCommand);

        private DbCommand CreateCommandByCommandType(CommandType commandType, string commandText)
        {
            const string whitelist = @"[^a-zA-Z0-9]";
            string commandtx = Regex.Replace(commandText, whitelist, "");
            DbCommand command = _dbProviderFactory.CreateCommand();
            if (command != null)
            {
                command.CommandType = commandType;
                command.CommandText = commandtx ;
                return command;
            }

            throw new ArgumentException($"Invalid Store procedure name: {commandText}");

        }

        private int DoUpdateDataSet(UpdateBehavior behavior,
                                  DataSet dataSet, string tableName, DbCommand insertCommand,
                                  DbCommand updateCommand, DbCommand deleteCommand)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentException("The value can not be null or an empty string.", "tableName");
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            if (insertCommand == null && updateCommand == null && deleteCommand == null)
            {
                throw new ArgumentException("At least one command must be initialized");
            }

            using (DbDataAdapter adapter = GetDataAdapter(behavior))
            {
                IDbDataAdapter explicitAdapter = adapter;
                if (insertCommand != null)
                {
                    explicitAdapter.InsertCommand = insertCommand;
                }
                if (updateCommand != null)
                {
                    explicitAdapter.UpdateCommand = updateCommand;
                }
                if (deleteCommand != null)
                {
                    explicitAdapter.DeleteCommand = deleteCommand;
                }

                try
                {
                    int rows = adapter.Update(dataSet.Tables[tableName] ?? throw new InvalidOperationException());
                    return rows;
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                    throw;
                }
            }
        }

        private void DoLoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            if (tableNames == null) throw new ArgumentNullException("tableNames");
            if (tableNames.Length == 0)
            {
                throw new ArgumentException("The table name array used to map results to user-specified table names cannot be empty.", "tableNames");
            }
            for (int i = 0; i < tableNames.Length; i++)
            {
                if (string.IsNullOrEmpty(tableNames[i])) throw new ArgumentException("The value can not be null or an empty string.", string.Concat("tableNames[", i, "]"));
            }

            using (DbDataAdapter adapter = GetDataAdapter(UpdateBehavior.Standard))
            {
                ((IDbDataAdapter)adapter).SelectCommand = command;

                string systemCreatedTableNameRoot = "Table";
                for (int i = 0; i < tableNames.Length; i++)
                {
                    string systemCreatedTableName = (i == 0)
                        ? systemCreatedTableNameRoot
                        : systemCreatedTableNameRoot + i;

                    adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                }

                adapter.Fill(dataSet);
            }
        }

        private object DoExecuteScalar(DbCommand command)
        {
            object returnValue = command.ExecuteScalar();
            return returnValue;
        }

        private int DoExecuteNonQuery(DbCommand command)
        {
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        }

        private IDataReader DoExecuteReader(DbCommand command, CommandBehavior cmdBehavior)
        {
            try
            {
                IDataReader reader = command.ExecuteReader(cmdBehavior);
                return reader;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        }

        private DbTransaction BeginTransaction(DbConnection connection)
        {
            DbTransaction tran = connection.BeginTransaction();
            return tran;
        }

        private void RollbackTransaction(DbTransaction tran)
        {
            tran.Rollback();
        }

        private void CommitTransaction(DbTransaction tran)
        {
            tran.Commit();
        }

        private void AssignParameterValues(DbCommand command, object[] values)
        {
            int parameterIndexShift = UserParametersStartIndex();
            for (int i = 0; i < values.Length; i++)
            {
                IDataParameter parameter = command.Parameters[i + parameterIndexShift];
                SetParameterValue(command, parameter.ParameterName, values[i]);
            }
        }

        protected DbParameter CreateParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter param = CreateParameter(name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        protected DbParameter CreateParameter(string name)
        {
            DbParameter param = _dbProviderFactory.CreateParameter();
            if (param != null)
            {
                param.ParameterName = BuildParameterName(name);

                return param;
            }
            _logger.Error($"Parameter {name} is null");
            throw new InvalidOperationException("Parameter null");
        }

        protected virtual int UserParametersStartIndex()
        {
            return 0;
        }

        protected virtual void ConfigureParameter(DbParameter param, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

    }

    public enum UpdateBehavior
    {
        /// <summary>
        /// No interference with the DataAdapter's Update command. If Update encounters
        /// an error, the update stops.  Additional rows in the Datatable are uneffected.
        /// </summary>
        Standard,
        /// <summary>
        /// If the DataAdapter's Update command encounters an error, the update will
        /// continue. The Update command will try to update the remaining rows. 
        /// </summary>
        Continue,
        /// <summary>
        /// If the DataAdapter encounters an error, all updated rows will be rolled back.
        /// </summary>
        Transactional
    }
}
