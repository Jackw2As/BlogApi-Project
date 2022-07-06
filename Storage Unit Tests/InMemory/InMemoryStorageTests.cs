using BlogAPI.Storage.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Storage.InMemory
{
    public class InMemoryStorageTests
    {
        private SqliteConnection _connection;
        private readonly DbContextOptions _contextOptions;

        public InMemoryStorageTests()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<InMemoryDBContext>()
                .UseSqlite(_connection)
                .Options;
        }

        public void Dispose() => _connection.Dispose();



        #region GetByID
        [Fact]
        public void ShouldGetDataObjectwithIDReturnDataObject()
        {
            //Arrange
            DataObject[] data = CreateDataObjectArray();
            var repository = CreateRepository(data);

            //Act
            var dataInDatabase = repository.GetByID(data[0].ID);

            //Assert
            Assert.NotNull(data);
            Assert.Equal(data[0].ID, dataInDatabase.ID);

            Dispose();
        }
        [Fact]
        public void ShouldGetDataObjectwithIDReturnArgumentException()
        {
            DataObject[] data = CreateDataObjectArray();
            InMemoryRepository<DataObject> repository = CreateRepository(data);

            Assert.ThrowsAny<ArgumentException>(() => repository.GetByID(Guid.NewGuid()));

            Dispose();
        }

        
        #endregion

        #region GetByQuery
        [Fact]
        public void ShouldReturnAListofDataObjects()
        {
            //arrange
            DataObject[] data = CreateDataObjectArray();

            while(data.Where((data) => data.ID.ToString()[0] == 'a').Count() > 1)
            {
                data = CreateDataObjectArray();
            }

            var repository = CreateRepository(data);
            Func<DataObject, bool> query = (data) => data.ID.ToString()[0] == 'a';

            var QuerriedDataObjectList = from DataObject in data
                                         where DataObject.ID.ToString()[0] == 'a'
                                         select DataObject;


            //act
            var results = repository.GetByQuery(query);

            //assert
            foreach (var item in QuerriedDataObjectList)
            {
                Assert.Contains(item, results);
            }

            Dispose();
        }

        
        #endregion

        #region Save
        #endregion

        #region Modifiy
        #endregion

        #region Delete
        [Fact]
        public void ShouldDeleteData()
        {
            //arranage
            var data = CreateDataObjectArray();
            var repository = CreateRepository(data);

            var deletedItemID = data[0].ID;
            Assert.NotNull(repository.GetByID(deletedItemID));
            
            //act;
            var success = repository.Delete(deletedItemID);

            //assert
            Assert.True(success, "For some reason Delete did not occur");

            Assert.Throws<ArgumentException>(() => repository.GetByID(deletedItemID));
            // We expect an arguement exception because the repository returns 
            // an arguement exception when it can't find the object being requested.

            Dispose();
        }
        #endregion


        #region Helpers
        private static DataObject[] CreateDataObjectArray()
        {
            DataObject[] data =
            {
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
            };

            return data;
        }

        private InMemoryRepository<DataObject> CreateRepository(DataObject[] data)
        {
            return new(data, _contextOptions);
        }
        #endregion
    }
}
