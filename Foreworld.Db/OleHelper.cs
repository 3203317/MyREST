using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Foreworld.Db
{
    public static class OleHelper
    {
        private static void AttachParameters(OleDbCommand command, OleDbParameter[] sqlParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            foreach (OleDbParameter sqlParameter in sqlParameters)
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

        private static void PrepareCommand(OleDbCommand command, OleDbConnection connection, OleDbTransaction transaction,
                                         CommandType commandType, string commandText, OleDbParameter[] sqlParameters,
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

        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText, params OleDbParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return ExecuteDataSet(connection, commandType, commandText, sqlParameters);
            }
        }

        public static DataSet ExecuteDataSet(OleDbConnection connection, CommandType commandType, string commandText, params OleDbParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            OleDbCommand command = new OleDbCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, sqlParameters, out mustCloseConnection);

            using (OleDbDataAdapter sqlDataAdapter = new OleDbDataAdapter(command))
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
                                          params OleDbParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return ExecuteScalar(connection, commandType, commandText, sqlParameters);
            }
        }

        public static object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText,
                                          params OleDbParameter[] sqlParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            OleDbCommand command = new OleDbCommand();

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
