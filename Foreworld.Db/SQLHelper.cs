using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

//namespace Microsoft.ApplicationBlocks.Data
namespace Foreworld.Db
{
    public static class SqlHelper
    {
        #region AttachParameters 该函数用于将所有必要的 SqlParameter 对象连接到正在运行的 SqlCommand

        // AttachParameters(SqlCommand command, SqlParameter[] sqlParameters)
        private static void AttachParameters(SqlCommand command, SqlParameter[] sqlParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            foreach (SqlParameter sqlParameter in sqlParameters)
            {
                if (sqlParameter != null)
                {
                    if ((sqlParameter.Direction == ParameterDirection.Input ||
                         sqlParameter.Direction == ParameterDirection.InputOutput) && (sqlParameter.Value == null))
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(sqlParameter);
                }
            }
        }

        #endregion

        #region AssignParameterValues 该函数用于为 SqlParameter 对象赋值

        // AssignParameterValues(SqlParameter[] sqlParameters, DataRow dataRow)
        private static void AssignParameterValues(SqlParameter[] sqlParameters, DataRow dataRow)
        {
            if ((sqlParameters == null) || (dataRow == null))
            {
                return;
            }

            int i = 0;

            foreach (SqlParameter sqlParameter in sqlParameters)
            {
                if (sqlParameter.ParameterName == null || sqlParameter.ParameterName.Length <= 1)
                {
                    throw new Exception(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                            i, sqlParameter.ParameterName));
                }

                if (dataRow.Table.Columns.IndexOf(sqlParameter.ParameterName.Substring(1)) != -1)
                {
                    sqlParameter.Value = dataRow[sqlParameter.ParameterName.Substring(1)];
                }
                i++;
            }
        }


        // AssignParameterValues(SqlParameter[] sqlParameters, object[] parameterValues)
        private static void AssignParameterValues(SqlParameter[] sqlParameters, object[] parameterValues)
        {
            if ((sqlParameters == null) || (parameterValues == null))
            {
                return;
            }
            if (sqlParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }
            for (int i = 0, j = sqlParameters.Length; i < j; i++)
            {
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter parameter = (IDbDataParameter)parameterValues[i];
                    if (parameter.Value == null)
                    {
                        sqlParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        sqlParameters[i].Value = parameter.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    sqlParameters[i].Value = DBNull.Value;
                }
                else
                {
                    sqlParameters[i].Value = parameterValues[i];
                }
            }
        }

        #endregion

        #region PrepareCommand 该函数用于对命令的属性（如连接、事务环境等）进行初始化

        // PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] sqlParameters, out bool mustCloseConnection )
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction,
                                           CommandType commandType, string commandText, SqlParameter[] sqlParameters,
                                           out bool mustCloseConnection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (command == null || commandText.Length == 0)
            {
                throw new ArgumentNullException("commandText");
            }
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;
            command.CommandText = commandText;

            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException(
                        "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }
                command.Transaction = transaction;
            }

            command.CommandType = commandType;

            if (sqlParameters != null)
            {
                AttachParameters(command, sqlParameters);
            }
            return;
        }

        #endregion

        #region ExecuteNonQuery 此方法用于执行不返回任何行或值的命令，这些命令通常用于执行数据库更新，但也可用于返回存储过程的输出参数

        // ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        // ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText,
                                          params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteNonQuery(connection, commandType, commandText, sqlParameters);
            }
        }

        // ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText,
                                          params SqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, sqlParameters, out mustCloseConnection);

            int retval = command.ExecuteNonQuery();

            command.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }
            return retval;
        }

        // ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        // ExecuteNonQuery(string connectionString, string storedProcedureName, params object[] parameterValues)
        public static int ExecuteNonQuery(string connectionString, string storedProcedureName,
                                          params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString,
                                                                                         storedProcedureName);
                AssignParameterValues(sqlParameters, parameterValues);
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteNonQuery(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        public static int ExecuteNonQuery(SqlConnection connection, string storedProcedureName,
                                          params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection,
                                                                                         storedProcedureName);
                AssignParameterValues(sqlParameters, parameterValues);
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText,
                                          params SqlParameter[] sqlParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }

            SqlCommand command = new SqlCommand();

            bool mustCloseConnection = false;

            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, sqlParameters,
                           out mustCloseConnection);

            int retval = command.ExecuteNonQuery();

            command.Parameters.Clear();

            return retval;
        }

        // ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        // ExecuteNonQuery(SqlTransaction transaction, string storedProcedureName, params object[] parameterValues)
        public static int ExecuteNonQuery(SqlTransaction transaction, string storedProcedureName,
                                          params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
                throw new ArgumentNullException("storedProcedureName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                                                                                         storedProcedureName);
                AssignParameterValues(sqlParameters, parameterValues);
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteDataSet 此方法返回 DataSet 对象，该对象包含由某一命令返回的结果集

        // ExecuteDataSet(string connectionString, CommandType commandType, string commandText)
        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataSet(connectionString, commandType, commandText, null);
        }

        // ExecuteDataSet(string connectionString, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText,
                                             params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteDataSet(connection, commandType, commandText, sqlParameters);
            }
        }

        // ExecuteDataSet(string connectionString, string storedProcedureName, params object[] parameterValues)
        public static DataSet ExecuteDataSet(string connectionString, string storedProcedureName,
                                             params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteDataSet(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteDataSet(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteDataSet(SqlConnection connection, CommandType commandType, string commandText)
        public static DataSet ExecuteDataSet(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataSet(connection, commandType, commandText, null);
        }

        // ExecuteDataSet(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static DataSet ExecuteDataSet(SqlConnection connection, CommandType commandType, string commandText,
                                             params SqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, sqlParameters, out mustCloseConnection);

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet);

                command.Parameters.Clear();

                if (mustCloseConnection)
                {
                    connection.Close();
                }

                return dataSet;
            }
        }

        // ExecuteDataSet(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        public static DataSet ExecuteDataSet(SqlConnection connection, string storedProcedureName,
                                             params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteDataSet(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteDataSet(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteDataSet(SqlTransaction transaction, CommandType commandType, string commandText)
        public static DataSet ExecuteDataSet(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataSet(transaction, commandType, commandText, null);
        }

        // ExecuteDataSet(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static DataSet ExecuteDataSet(SqlTransaction transaction, CommandType commandType, string commandText,
                                             params SqlParameter[] sqlParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, sqlParameters,
                           out mustCloseConnection);

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                command.Parameters.Clear();
                return dataSet;
            }
        }

        // ExecuteDataSet(SqlTransaction transaction, string storedProcedureName, params object[] parameterValues)
        public static DataSet ExecuteDataSet(SqlTransaction transaction, string storedProcedureName,
                                             params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteDataSet(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteDataSet(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteReader 此方法用于返回 SqlDataReader 对象，该对象包含由某一命令返回的结果集

        // ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] sqlParameters, SqlConnectionOwnership connectionOwnership)
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction,
                                                   CommandType commandType, string commandText,
                                                   SqlParameter[] sqlParameters,
                                                   SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            bool mustCloseConnection = false;

            SqlCommand command = new SqlCommand();
            try
            {
                PrepareCommand(command, connection, transaction, commandType, commandText, sqlParameters,
                               out mustCloseConnection);

                SqlDataReader dataReader;

                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = command.ExecuteReader();
                }
                else
                {
                    dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }

                bool canClear = true;

                foreach (SqlParameter sqlParameter in command.Parameters)
                {
                    if (sqlParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    command.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                throw;
            }
        }

        // ExecuteReader(string connectionString, CommandType commandType, string commandText)
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, null);
        }

        // ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText,
                                                  params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                return ExecuteReader(connection, null, commandType, commandText, sqlParameters,
                                     SqlConnectionOwnership.Internal);
            }
            catch
            {
                if (connection != null) connection.Close();
                throw;
            }
        }

        // ExecuteReader(string connectionString, string storedProcedureName, params object[] parameterValues)
        public static SqlDataReader ExecuteReader(string connectionString, string storedProcedureName,
                                                  params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        // ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText,
                                                  params SqlParameter[] sqlParameters)
        {
            return ExecuteReader(connection, null, commandType, commandText, sqlParameters,
                                 SqlConnectionOwnership.External);
        }

        // ExecuteReader(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        public static SqlDataReader ExecuteReader(SqlConnection connection, string storedProcedureName,
                                                  params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType,
                                                  string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        // ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType,
                                                  string commandText, params SqlParameter[] sqlParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, sqlParameters,
                                 SqlConnectionOwnership.External);
        }

        // ExecuteReader(SqlTransaction transaction, string storedProcedureName, params object[] parameterValues)
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string storedProcedureName,
                                                  params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteScalar 此方法返回一个值，该值始终是该命令返回的第一行的第一列

        // ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        // ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText,
                                           params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteScalar(connection, commandType, commandText, sqlParameters);
            }
        }

        // ExecuteScalar(string connectionString, string storedProcedureName, params object[] parameterValues)
        public static object ExecuteScalar(string connectionString, string storedProcedureName,
                                           params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        // ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText,
                                           params SqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SqlCommand command = new SqlCommand();

            bool mustCloseConnection = false;

            PrepareCommand(command, connection, null, commandType, commandText, sqlParameters, out mustCloseConnection);

            object retval = command.ExecuteScalar();

            command.Parameters.Clear();

            if (mustCloseConnection)
            {
                connection.Close();
            }

            return retval;
        }

        // ExecuteScalar(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        public static object ExecuteScalar(SqlConnection connection, string storedProcedureName,
                                           params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        // ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText,
                                           params SqlParameter[] sqlParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }

            SqlCommand command = new SqlCommand();

            bool mustCloseConnection = false;

            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, sqlParameters,
                           out mustCloseConnection);

            object retval = command.ExecuteScalar();

            command.Parameters.Clear();

            return retval;
        }

        // ExecuteScalar(SqlTransaction transaction, string storedProcedureName, params object[] parameterValues)
        public static object ExecuteScalar(SqlTransaction transaction, string storedProcedureName,
                                           params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteScalar(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteXmlReader 此方法返回 FOR XML 查询的 XML 片段

        // ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(connection, commandType, commandText, null);
        }

        // ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText,
                                                 params SqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            bool mustCloseConnection = false;

            SqlCommand command = new SqlCommand();

            try
            {
                PrepareCommand(command, connection, null, commandType, commandText, sqlParameters,
                               out mustCloseConnection);

                XmlReader retval = command.ExecuteXmlReader();

                command.Parameters.Clear();

                return retval;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                throw;
            }
        }

        // ExecuteXmlReader(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        public static XmlReader ExecuteXmlReader(SqlConnection connection, string storedProcedureName,
                                                 params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteXmlReader(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        // ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] sqlParameters)
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText,
                                                 params SqlParameter[] sqlParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }

            SqlCommand command = new SqlCommand();

            bool mustCloseConnection = false;

            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, sqlParameters,
                           out mustCloseConnection);

            XmlReader retval = command.ExecuteXmlReader();

            command.Parameters.Clear();

            return retval;
        }

        // ExecuteXmlReader(SqlTransaction transaction, string storedProcedureName, params object[] parameterValues)
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string storedProcedureName,
                                                 params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region FillDataSet

        // FillDataSet(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        public static void FillDataSet(string connectionString, CommandType commandType, string commandText,
                                       DataSet dataSet, string[] tableNames)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                FillDataSet(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        // FillDataSet(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] sqlParameters)
        public static void FillDataSet(string connectionString, CommandType commandType,
                                       string commandText, DataSet dataSet, string[] tableNames,
                                       params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                FillDataSet(connection, commandType, commandText, dataSet, tableNames, sqlParameters);
            }
        }

        // FillDataSet(string connectionString, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        public static void FillDataSet(string connectionString, string storedProcedureName,
                                       DataSet dataSet, string[] tableNames,
                                       params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                FillDataSet(connection, storedProcedureName, dataSet, tableNames, parameterValues);
            }
        }

        // FillDataSet(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        public static void FillDataSet(SqlConnection connection, CommandType commandType,
                                       string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataSet(connection, commandType, commandText, dataSet, tableNames, null);
        }

        // FillDataSet(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] sqlParameters)
        public static void FillDataSet(SqlConnection connection, CommandType commandType,
                                       string commandText, DataSet dataSet, string[] tableNames,
                                       params SqlParameter[] sqlParameters)
        {
            FillDataSet(connection, null, commandType, commandText, dataSet, tableNames, sqlParameters);
        }

        // FillDataSet(SqlConnection connection, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        public static void FillDataSet(SqlConnection connection, string storedProcedureName,
                                       DataSet dataSet, string[] tableNames,
                                       params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                FillDataSet(connection, CommandType.StoredProcedure, storedProcedureName, dataSet, tableNames,
                            sqlParameters);
            }
            else
            {
                FillDataSet(connection, CommandType.StoredProcedure, storedProcedureName, dataSet, tableNames);
            }
        }

        // FillDataSet(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        public static void FillDataSet(SqlTransaction transaction, CommandType commandType,
                                       string commandText,
                                       DataSet dataSet, string[] tableNames)
        {
            FillDataSet(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        // FillDataSet(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] sqlParameters)
        public static void FillDataSet(SqlTransaction transaction, CommandType commandType,
                                       string commandText, DataSet dataSet, string[] tableNames,
                                       params SqlParameter[] sqlParameters)
        {
            FillDataSet(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames,
                        sqlParameters);
        }

        // FillDataSet(SqlTransaction transaction, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        public static void FillDataSet(SqlTransaction transaction, string storedProcedureName,
                                       DataSet dataSet, string[] tableNames,
                                       params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection,
                                                                                         storedProcedureName);

                AssignParameterValues(sqlParameters, parameterValues);

                FillDataSet(transaction, CommandType.StoredProcedure, storedProcedureName, dataSet, tableNames,
                            sqlParameters);
            }
            else
            {
                FillDataSet(transaction, CommandType.StoredProcedure, storedProcedureName, dataSet, tableNames);
            }
        }

        // FillDataSet(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] sqlParameters)
        private static void FillDataSet(SqlConnection connection, SqlTransaction transaction, CommandType commandType,
                                        string commandText, DataSet dataSet, string[] tableNames,
                                        params SqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, sqlParameters,
                           out mustCloseConnection);

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0)
                        {
                            throw new ArgumentException(
                                "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.",
                                "tableNames");
                        }
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }
                dataAdapter.Fill(dataSet);
                command.Parameters.Clear();
            }

            if (mustCloseConnection)
            {
                connection.Close();
            }
        }

        #endregion

        #region UpdateDataSet

        // UpdateDataSet(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        public static void UpdateDataSet(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand,
                                         DataSet dataSet, string tableName)
        {
            if (insertCommand == null)
            {
                throw new ArgumentNullException("insertCommand");
            }
            if (deleteCommand == null)
            {
                throw new ArgumentNullException("deleteCommand");
            }
            if (updateCommand == null)
            {
                throw new ArgumentNullException("updateCommand");
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                dataAdapter.Update(dataSet, tableName);

                dataSet.AcceptChanges();
            }
        }

        #endregion

        #region CreateCommand

        // CreateCommand(SqlConnection connection, string storedProcedureName, params string[] sourceColumns)
        public static SqlCommand CreateCommand(SqlConnection connection, string storedProcedureName, params string[] sourceColumns)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                for (int index = 0; index < sourceColumns.Length; index++)
                {
                    sqlParameters[index].SourceColumn = sourceColumns[index];
                }

                AttachParameters(command, sqlParameters);
            }

            return command;
        }

        #endregion

        #region ExecuteNonQueryTypedParams

        // ExecuteNonQueryTypedParams(String connectionString, String storedProcedureName, DataRow dataRow)
        public static int ExecuteNonQueryTypedParams(String connectionString, String storedProcedureName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteNonQueryTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        public static int ExecuteNonQueryTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteNonQueryTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteDataSetTypedParams

        // ExecuteDataSetTypedParams(string connectionString, String storedProcedureName, DataRow dataRow)
        public static DataSet ExecuteDataSetTypedParams(string connectionString, String storedProcedureName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteDataSet(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataSet(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteDataSetTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        public static DataSet ExecuteDataSetTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteDataSet(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataSet(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteDataSetTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        public static DataSet ExecuteDataSetTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteDataSet(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataSet(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteReaderTypedParams

        // ExecuteReaderTypedParams(String connectionString, String storedProcedureName, DataRow dataRow)
        public static SqlDataReader ExecuteReaderTypedParams(String connectionString, String storedProcedureName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteReaderTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteReaderTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteScalarTypedParams

        // ExecuteScalarTypedParams(String connectionString, String storedProcedureName, DataRow dataRow)
        public static object ExecuteScalarTypedParams(String connectionString, String storedProcedureName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteScalarTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        public static object ExecuteScalarTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteScalarTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        public static object ExecuteScalarTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion

        #region ExecuteXmlReaderTypedParams

        // ExecuteXmlReaderTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String storedProcedureName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        // ExecuteXmlReaderTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String storedProcedureName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                SqlParameter[] sqlParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, storedProcedureName);

                AssignParameterValues(sqlParameters, dataRow);

                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, storedProcedureName, sqlParameters);
            }
            else
            {
                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, storedProcedureName);
            }
        }

        #endregion
    }

    internal enum SqlConnectionOwnership
    {
        Internal,
        External
    }

    internal static class SqlHelperParameterCache
    {
        private static readonly Hashtable parameterCache = Hashtable.Synchronized(new Hashtable());

        // DiscoverSpParameterSet(SqlConnection connection, string storedProcedureName, bool includeReturnValueParameter)
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string storedProcedureName,
                                                             bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("Connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("StoredProcedureName");
            }
            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlCommandBuilder.DeriveParameters(command);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                command.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[command.Parameters.Count];

            command.Parameters.CopyTo(discoveredParameters, 0);

            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        // CloneParameters(SqlParameter[] originalParameters)
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        // CacheParameterSet(string connectionString, string commandText, params SqlParameter[] sqlParameters)
        public static void CacheParameterSet(string connectionString, string commandText,
                                             params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("ConnectionString");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("CommandText");
            }

            string hashKey = connectionString + ":" + commandText;

            parameterCache[hashKey] = sqlParameters;
        }

        // GetCachedParameterSet(string connectionString, string commandText)
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("ConnectionString");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("CommandText");
            }

            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = parameterCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        // GetSpParameterSet(string connectionString, string storedProcedureName)
        public static SqlParameter[] GetSpParameterSet(string connectionString, string storedProcedureName)
        {
            return GetSpParameterSet(connectionString, storedProcedureName, false);
        }

        // GetSpParameterSet(string connectionString, string storedProcedureName, bool includeReturnValueParameter)
        public static SqlParameter[] GetSpParameterSet(string connectionString, string storedProcedureName,
                                                       bool includeReturnValueParameter)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("ConnectionString");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("StoredProcedureName");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, storedProcedureName, includeReturnValueParameter);
            }
        }

        // GetSpParameterSet(SqlConnection connection, string storedProcedureName)
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string storedProcedureName)
        {
            return GetSpParameterSet(connection, storedProcedureName, false);
        }

        // GetSpParameterSet(SqlConnection connection, string storedProcedureName, bool includeReturnValueParameter)
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string storedProcedureName,
                                                         bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("Connection");
            }
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, storedProcedureName, includeReturnValueParameter);
            }
        }

        // GetSpParameterSetInternal(SqlConnection connection, string storedProcedureName, bool includeReturnValueParameter)
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string storedProcedureName,
                                                                bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("Connection");
            }
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("StoredProcedureName");
            }

            string hashKey = connection.ConnectionString + ":" + storedProcedureName +
                             (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters = parameterCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, storedProcedureName,
                                                                     includeReturnValueParameter);
                parameterCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }
    }
}