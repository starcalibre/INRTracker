using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using FluentAssertions;
using INRTracker.Models;
using NUnit.Framework;
using INRTracker.Services;
using INRTracker.Tests.TestData;

namespace INRTracker.Tests
{
    /// <summary>
    /// Unit test fixture for INREntryRepositorySqlite. These tests are to check that the CRUD
    /// operations of the repository beahves and persists data as expected. Tests are carried out
    /// on a test database that's written to disk, and subsequently removed after tests are
    /// complete. This approach was chosen over an in-memory implemenation to avoid any
    /// problems caused by the data being discarded when IDispose is called.
    /// </summary>
    [TestFixture]
    public class TestINREntryRepositorySqlite
    {
        #region Private Fields

        private readonly string _connString = ConfigurationManager
            .ConnectionStrings["test"].ConnectionString;

        private string _scriptsFolder = "TestData";
        private string _populateTestDbScript = "INREntrySqlite.sql";

        private INREntryRepositorySqlite _repo;

        #endregion

        #region Test Setup / Teardown

        [SetUp]
        public void SetUp()
        {
            _repo = new INREntryRepositorySqlite(_connString);
            ExecuteScript(_populateTestDbScript);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteTestDatabase();
        }

        #endregion

        #region Tests

        [Test, Description("Method returns expeddcted number of entries.")]
        public void TestGetINREntriesAsync_ExpectedCount()
        {
            var query = _repo.GetINREntriesAsync();
            query.Result.Count.Should().Be(3, "because there are three entries in the test database.");
        }

        [Test, Description("Method returns expected entry by ID.")]
        public void TestGetINREntryByIDAsync_ExpectedEntry()
        {
            var query = _repo.GetINREntryByIDAsync(1);
            query.Result.ShouldBeEquivalentTo(TestINREntryModels.FirstEntry);
        }

        [Test, Description("Method returns null if invalid ID queried.")]
        public void TestGetINREntryByIDAsync_ExpectedNull()
        {
            var query = _repo.GetINREntryByIDAsync(5);
            query.Result.Should().BeNull("because we queried an invalid entry ID.");
        }

        [Test, Description("Method returns new entry when added.")]
        public void AddINREntryAsync_ExpectedNewEntry()
        {
            // copy new entry for this test
            INREntry expectedAddedEntry = new INREntry()
            {
                INREntryID = 4,
                Date = TestINREntryModels.NewEntry.Date,
                INR = TestINREntryModels.NewEntry.INR,
                DoseMg = TestINREntryModels.NewEntry.DoseMg,
                DoseMgAlternating = TestINREntryModels.NewEntry.DoseMgAlternating
            };

            var query = _repo.AddINREntryAsync(TestINREntryModels.NewEntry);
            query.Result.ShouldBeEquivalentTo(expectedAddedEntry);
        }

        [Test, Description("Expected number of entries returned after successful addition.")]
        public void AddINREntryAsync_ExpectedNumberOfEntries()
        {
            var addQuery = _repo.AddINREntryAsync(TestINREntryModels.NewEntry);
            addQuery.Wait(); // let async add method finish

            var countQuery = _repo.GetINREntriesAsync();
            countQuery.Result.Count.Should().Be(4, "because we expect 4 entries in the database.");
        }

        [Test, Description("Method throws expected when entry with invalid data added.")]
        public void AddINREntryAsync__ExpectionWhenInvalidData()
        {
            Action invalidAdd = () =>
            {
                var query = _repo.AddINREntryAsync(TestINREntryModels.NewEntryInvalid);
                query.Wait();
            };

            invalidAdd.ShouldThrow<SQLiteException>("because we tried to add an entry with invalid data.");
        }

        [Test, Description("Method returns 1 when update successful.")]
        public void UpdateINREntryAsync_ReturnsOneWhenSuccessful()
        {
            var query = _repo.UpdateINREntryAsync(TestINREntryModels.UpdatedEntryValid);
            query.Result.Should().Be(1, "because one row was affected by the operation.");
        }

        [Test, Description("Method throws exception if invalid data passed when updating.")]
        public void UpdateINREntryAsync_ExpectionWhenInvalidData()
        {
            Action invalidUpdate = () =>
            {
                var query = _repo.UpdateINREntryAsync(TestINREntryModels.UpdatedEntryInvalid);
                query.Wait();
            };

            invalidUpdate.ShouldThrow<SQLiteException>("because we tried to update an entry with invalid data.");
        }

        [Test, Description("Method returns 1 when deletion successful.")]
        public void DeleteINREntryAsync_ReturnsOneWhenSuccessful()
        {
            var query = _repo.DeleteINREntryAsync(TestINREntryModels.ThirdEntry);
            query.Result.Should().Be(1, "because one row was affected by the operation.");
        }

        [Test, Description("Method returns 0 when deletion unsuccessful.")]
        public void DeleteINREntryAsync_ReturnsZeroWhenUnsuccessful()
        {
            var query = _repo.DeleteINREntryAsync(TestINREntryModels.EntryInvalidID);
            query.Result.Should().Be(0, "because zero rows were affected by the operation.");
        }

        [Test, Description("Expected number of entries returned after successful deletion.")]
        public void DeleteINREntryAsync_ExpectedNumberOfEntries()
        {
            var deleteQuery = _repo.DeleteINREntryAsync(TestINREntryModels.SecondEntry);
            deleteQuery.Wait(); // let async operation finish

            var countQuery = _repo.GetINREntriesAsync();
            countQuery.Result.Count.Should().Be(2, "because we expect 2 entries in the database.");
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Deletes the tempory test Sqlite database file.
        /// </summary>
        private void DeleteTestDatabase()
        {
            string dbPath;

            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                dbPath = conn.FileName;
            }

            File.Delete(dbPath);
        }

        /// <summary>
        /// Execute a SQL script script.
        /// </summary>
        /// <param name="script">The filename of the script to execute. Should reside in the Scripts folder.</param>
        private void ExecuteScript(string script)
        {
            string currentDir = TestContext.CurrentContext.TestDirectory;
            string scriptPath = Path.Combine(currentDir, _scriptsFolder, script);
            string scriptText = File.ReadAllText(scriptPath);

            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = scriptText;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}
