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
            DataObject[] data = { new() { ID = Guid.NewGuid() } };

            InMemoryRepository<DataObject> repository = new(data ,_contextOptions);

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

            DataObject[] data = { new() { ID = Guid.NewGuid() } };
            InMemoryRepository<DataObject> repository = new(data, _contextOptions);
            
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

            InMemoryRepository<DataObject> repository = new(data, _contextOptions);
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

        }

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
        #endregion
    }
}
