using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using MySql.Data.MySqlClient;

namespace Foreworld.Db
{
    public static class MySQLHelper
    {
        private static void AttachParameters(MySqlCommand command, MySqlParameter[] sqlParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            foreach (MySqlParameter sqlParameter in sqlParameters)
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

        private static void PrepareCommand(MySqlCommand command, MySqlConnection connection, MySqlTransaction transaction,
                                         CommandType commandType, string commandText, MySqlParameter[] sqlParameters,
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

        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteDataSet(connection, commandType, commandText, sqlParameters);
            }
        }

        public static DataSet ExecuteDataSet(MySqlConnection connection, CommandType commandType, string commandText, params MySqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            MySqlCommand command = new MySqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, sqlParameters, out mustCloseConnection);

            using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command))
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

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText,
                                          params MySqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteScalar(connection, commandType, commandText, sqlParameters);
            }
        }

        public static object ExecuteScalar(MySqlConnection connection, CommandType commandType, string commandText,
                                          params MySqlParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            MySqlCommand command = new MySqlCommand();

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
    }
}
