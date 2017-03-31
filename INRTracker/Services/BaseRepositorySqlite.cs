using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INRTracker.Services
{
    /// <summary>
    /// Base abstract class for DB connections to SQLite.
    /// </summary>
    public abstract class BaseRepositorySqlite
    {
        #region Protected Fields

        protected readonly string _connString;

        #endregion

        #region Constructor

        protected BaseRepositorySqlite(string connString)
        {
            _connString = connString;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Wrapper method for asynchronous connections to SQLite databases.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getData"></param>
        /// <returns></returns>
        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connString))
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }

        #endregion
    }
}