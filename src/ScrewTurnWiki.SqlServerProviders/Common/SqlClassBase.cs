using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace ScrewTurn.Wiki.Plugins.SqlCommon
{
    /// <summary>
    ///     Implements a base class for all SQL-based classes.
    /// </summary>
    public abstract class SqlClassBase
    {
        #region Utility Methods

        /// <summary>
        ///     Gets a value indicating whether a column contains nonexistent or missing value.
        /// </summary>
        /// <param name="reader">The <see cref="T:DbDataReader" />.</param>
        /// <param name="column">The name of the column.</param>
        /// <returns><c>true</c> if the column contains a nonexistent or missing value, <c>false</c> otherwise.</returns>
        protected bool IsDBNull(IDataReader reader, string column)
        {
            return reader.IsDBNull(reader.GetOrdinal(column));
        }

        /// <summary>
        ///     Gets the value of a nullable column.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="reader">The <see cref="T:DbDataReader" />.</param>
        /// <param name="column">The name of the column.</param>
        /// <param name="defaultValue">The default value to return when the column contains a null value.</param>
        /// <returns>The value.</returns>
        protected T GetNullableColumn<T>(IDataReader reader, string column, T defaultValue)
        {
            if (IsDBNull(reader, column)) return defaultValue;
            return (T)reader[column];
        }

        /// <summary>
        ///     Reads all the contents of a binary column.
        /// </summary>
        /// <param name="reader">The <see cref="T:DbDataReader" />.</param>
        /// <param name="column">The name of the column.</param>
        /// <param name="maxSize">The max size, in bytes, to read. If exceeded, <c>null</c> is returned.</param>
        /// <returns>The read bytes, or <c>null</c>.</returns>
        /// <remarks>This method buffers the data in memory; avoid reading data bigger than a few megabytes.</remarks>
        protected byte[] GetBinaryColumn(IDataReader reader, string column, int maxSize)
        {
            // 128 KB read buffer
            var buffer = new byte[131072];
            var tempResult = new byte[maxSize];

            var columnOrdinal = reader.GetOrdinal(column);

            long read = 0;
            long totalRead = 0;
            do
            {
                read = reader.GetBytes(columnOrdinal, totalRead, buffer, 0, buffer.Length);

                if (totalRead + read > maxSize) return null;

                if (read > 0)
                {
                    Buffer.BlockCopy(buffer, 0, tempResult, (int)totalRead, (int)read);
                }
                totalRead += read;
            } while (read > 0);

            // Copy tempBuffer in final array
            buffer = null;

            var result = new byte[totalRead];
            Buffer.BlockCopy(tempResult, 0, result, 0, result.Length);

            return result;
        }

        /// <summary>
        ///     Copies all the contents of a binary column into a <see cref="T:System.IO.Stream" />.
        /// </summary>
        /// <param name="reader">The <see cref="T:DbDataReader" />.</param>
        /// <param name="column">The name of the column.</param>
        /// <param name="stream">The destination <see cref="T:System.IO.Stream" />.</param>
        protected int ReadBinaryColumn(IDataReader reader, string column, Stream stream)
        {
            // 128 KB read buffer
            var buffer = new byte[131072];

            var columnOrdinal = reader.GetOrdinal(column);

            var read = 0;
            var totalRead = 0;
            do
            {
                read = (int)reader.GetBytes(columnOrdinal, totalRead, buffer, 0, buffer.Length);

                if (read > 0)
                {
                    stream.Write(buffer, 0, read);
                }

                totalRead += read;
            } while (read > 0);

            return totalRead;
        }

        /// <summary>
        ///     Logs an exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        protected abstract void LogException(Exception ex);

        /// <summary>
        ///     Executes a scalar command, then closes the connection.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="defaultValue">The default value of the return value, to use when the command fails.</param>
        /// <returns>The result.</returns>
        /// <remarks>The connection is closed after the execution.</remarks>
        protected T ExecuteScalar<T>(IDbCommand command, T defaultValue)
        {
            return ExecuteScalar(command, defaultValue, true);
        }

        /// <summary>
        ///     Executes a scalar command, then closes the connection.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="defaultValue">The default value of the return value, to use when the command fails.</param>
        /// <param name="close">A value indicating whether to close the connection after execution.</param>
        /// <returns>The result.</returns>
        protected T ExecuteScalar<T>(IDbCommand command, T defaultValue, bool close)
        {
            object temp = null;

            try
            {
                temp = command.ExecuteScalar();
            }
            catch (DbException dbex)
            {
                LogException(dbex);
            }
            finally
            {
                if (close)
                {
                    CloseConnection(command.Connection);
                }
            }

            if (temp != null)
            {
                return (T)temp;
            }
            return defaultValue;
        }

        /// <summary>
        ///     Executes a non-query command, then closes the connection.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The rows affected (-1 if the command failed).</returns>
        /// <remarks>The connection is closed after the execution.</remarks>
        protected int ExecuteNonQuery(IDbCommand command)
        {
            return ExecuteNonQuery(command, true);
        }

        /// <summary>
        ///     Executes a non-query command, then closes the connection if requested.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="close">A value indicating whether to close the connection after execution.</param>
        /// <returns>The rows affected (-1 if the command failed).</returns>
        protected int ExecuteNonQuery(IDbCommand command, bool close)
        {
            return ExecuteNonQuery(command, close, true);
        }

        /// <summary>
        ///     Executes a non-query command, then closes the connection if requested.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="close">A value indicating whether to close the connection after execution.</param>
        /// <param name="logError">A value indicating whether to log any error.</param>
        /// <returns>The rows affected (-1 if the command failed).</returns>
        protected int ExecuteNonQuery(IDbCommand command, bool close, bool logError)
        {
            var rows = -1;

            try
            {
                rows = command.ExecuteNonQuery();
            }
            catch (DbException dbex)
            {
                if (logError) LogException(dbex);
            }
            finally
            {
                if (close)
                {
                    CloseConnection(command.Connection);
                }
            }

            return rows;
        }

        /// <summary>
        ///     Executes a reader command, leaving the connection open.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="closeOnError">A value indicating whether to close the connection on error.</param>
        /// <returns>The data reader, or <c>null</c> if the command fails.</returns>
        protected IDataReader ExecuteReader(IDbCommand command, bool closeOnError)
        {
            IDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (DbException dbex)
            {
                LogException(dbex);
                if (closeOnError) CloseConnection(command.Connection);
            }

            return reader;
        }

        /// <summary>
        ///     Executes a reader command, leaving the connection open.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The data reader, or <c>null</c> if the command fails.</returns>
        /// <remarks>If the command fails, the connection is closed.</remarks>
        protected IDataReader ExecuteReader(IDbCommand command)
        {
            return ExecuteReader(command, true);
        }

        /// <summary>
        ///     Closes a connection, swallowing all exceptions.
        /// </summary>
        /// <param name="connection">The connection to close.</param>
        protected void CloseConnection(IDbConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Closes a reader, a command and the associated connection.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="reader">The reader.</param>
        protected void CloseReader(IDbCommand command, IDataReader reader)
        {
            try
            {
                reader.Close();
            }
            catch
            {
            }

            CloseConnection(command.Connection);
        }

        /// <summary>
        ///     Closes a reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        protected void CloseReader(IDataReader reader)
        {
            try
            {
                reader.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Begins a transaction.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>The transaction.</returns>
        protected IDbTransaction BeginTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        /// <summary>
        ///     Commits a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        protected void CommitTransaction(IDbTransaction transaction)
        {
            // Commit sets transaction.Connection to null
            var connection = transaction.Connection;
            transaction.Commit();
            transaction.Dispose();
            CloseConnection(connection);
        }

        /// <summary>
        ///     Rolls back a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        protected void RollbackTransaction(IDbTransaction transaction)
        {
            // Rollback sets transaction.Connection to null
            var connection = transaction.Connection;
            try
            {
                transaction.Rollback();
            }
            catch
            {
            }
            transaction.Dispose();
            CloseConnection(connection);
        }

        #endregion
    }
}