using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using INRTracker.Models;

namespace INRTracker.Services
{
    public class INREntryRepositorySqlite : BaseRepositorySqlite, IINREntryRepository
    {
        #region Procedures

        /// <summary>
        /// Private nested class to store procedures. This is used due to SQLite's lack of a stored
        /// procedure functionality.
        /// </summary>
        private static class Procedures
        {
            public static string ExecuteBuildTable = "CREATE TABLE IF NOT EXISTS INREntries(" +
                                                     "INREntryID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                     "Date TEXT NOT NULL, " +
                                                     "INR REAL NOT NULL CHECK(INR >= 0), " +
                                                     "DoseMg REAL NOT NULL CHECK(DoseMg >= 0), " +
                                                     "DoseMgAlternating REAL NOT NULL CHECK(DoseMgAlternating >= 0)" +
                                                     ");";
            public static string QueryAllEntries = "SELECT * FROM INREntries;";
            public static string QueryEntryByID = "SELECT * FROM INREntries WHERE INREntryID = @id";
            public static string QueryAddEntry = "INSERT INTO INREntries (Date, INR, DoseMg, DoseMgAlternating)" +
                                                 "VALUES (@Date, @INR, @DoseMg, @DoseMgAlternating);" +
                                                 "SELECT * FROM INREntries WHERE INREntryID = last_insert_rowid();";
            public static string ExecuteUpdateEntry = "UPDATE INREntries " +
                                                      "SET Date = @Date, INR = @INR, DoseMg = @DoseMG, DoseMgAlternating = @DoseMgAlternating " +
                                                      "WHERE INREntryID = @INREntryID";
            public static string ExecuteDeleteEntry = "DELETE FROM INREntries WHERE INREntryID = @INREntryID";
        }

        #endregion

        #region Constructor

        public INREntryRepositorySqlite(string connString) : base(connString)
        {
            // create the table
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = Procedures.ExecuteBuildTable;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Methods

        public async Task<List<INREntry>> GetINREntriesAsync()
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<INREntry>(Procedures.QueryAllEntries);
                return query.ToList();
            });
        }

        public async Task<INREntry> GetINREntryByIDAsync(long id)
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<INREntry>(Procedures.QueryEntryByID, new {id});
                return query.SingleOrDefault();
            });
        }

        public async Task<INREntry> AddINREntryAsync(INREntry newEntry)
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<INREntry>(Procedures.QueryAddEntry, newEntry);
                return query.SingleOrDefault();
            });
        }

        public async Task<int> UpdateINREntryAsync(INREntry updatedEntry)
        {
            return await WithConnection(async conn =>
                    await conn.ExecuteAsync(Procedures.ExecuteUpdateEntry, updatedEntry));
        }

        public async Task<int> DeleteINREntryAsync(INREntry entryToDelete)
        {
            return await WithConnection(async conn =>
                    await conn.ExecuteAsync(Procedures.ExecuteDeleteEntry, entryToDelete));
        }

        #endregion
    }
}
