using BlogAPI.Storage.DatabaseModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Storage.InMemory
{

    public class BaseInMemoryStorageTest : IDisposable
    {

        private SqliteConnection _connection;
        private readonly DbContextOptions _contextOptions;

        protected IEnumerable<MockDatabaseObject> MockObjects;

        public BaseInMemoryStorageTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<InMemoryDBContext>()
                .UseSqlite(_connection)
                .Options;

            SeedData();
        }


        private void SeedData()
        {
            using var context = new MockInMemoryDBContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                context.AddRange(CreateMockObjects());

                context.SaveChanges();
            }
        }

        public void Dispose() => _connection.Dispose();

        #region Helpers
        private IEnumerable<MockDatabaseObject> CreateMockObjects()
        {
            MockDatabaseObject[] data =
            {
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
                new() { ID = Guid.NewGuid().ToString() },
            };

            MockObjects = data;
            return data;
        }

        protected MockInMemoryDBContext CreateContext => new(_contextOptions);
        protected MockInMemoryRepository CreateRepository()
        {
            return new MockInMemoryRepository(CreateContext);
        }
        #endregion
    }

    [Collection("Base Tests")]
    public class GetByQuery : BaseInMemoryStorageTest
    {
        [Fact]
        public void ShouldReturnAListofDataObjects()
        {
            //arrange
            var data = MockObjects;

            while (data.Where((data) => data.ID.ToString()[0] == 'a').Count() > 2)
            {
                data = MockObjects;
            }

            var repository = CreateRepository();
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
    }

    [Collection("Base Tests")]
    public class GetByID : BaseInMemoryStorageTest
    {
        protected MockDatabaseObject[] Data { get; init; }
        public GetByID()
        {
            Data = MockObjects.ToArray();
        }
        #region GetByID
        [Fact]
        public void ShouldGetDataObjectwithIDReturnDataObject()
        {
            //Arrange
            var repository = CreateRepository();

            //Act
            var dataInDatabase = repository.GetByID(Data[0].ID);

            //Assert
            Assert.NotNull(Data);
            Assert.Equal(Data[0].ID, dataInDatabase.ID);
        }
        [Fact]
        public void ShouldGetDataObjectwithIDReturnArgumentException()
        {
            MockInMemoryRepository repository = CreateRepository();

            Assert.ThrowsAny<ArgumentException>(() => repository.GetByID(Guid.NewGuid().ToString()));
        }

        #endregion
    }

    [Collection("Base Tests")]
    public class Save : BaseInMemoryStorageTest
    {
        [Fact]
        public void ShouldAddNewData()
        {
            //arranage
            var repository = CreateRepository();

            //act
            var newObject = new MockDatabaseObject() { ID = Guid.NewGuid().ToString() };
            bool success = repository.Save(newObject);

            //Assert
            Assert.True(success, "Repository did not save for some reason");
        }
    }

    [Collection("Base Tests")]
    public class Modify : BaseInMemoryStorageTest
    {
        [Fact]
        public void ShouldModifyData()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var item = repository.GetByID(data.First().ID);

            //act
            item.ManipulateMe = false;
            repository.Modify(item);

            //assert
            var collection = repository.GetByQuery((obj) => obj.ManipulateMe == false);

            Assert.NotEmpty(collection);
            Assert.Contains(item, collection);
            Assert.InRange(collection.Count(), 1, 1);
        }

    }

    [Collection("Base Tests")]
    public class Delete : BaseInMemoryStorageTest
    {
        [Fact]
        public void ShouldDeleteData()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var deletedItemID = data.First().ID;
            Assert.NotNull(repository.GetByID(deletedItemID));

            //act;
            var success = repository.Delete(deletedItemID);

            //assert
            Assert.True(success, "For some reason Delete did not occur");

            Assert.Throws<ArgumentException>(() => repository.GetByID(deletedItemID));
            // We expect an arguement exception because the repository returns 
            // an arguement exception when it can't find the object being requested.
        }
    }

    [Collection("Base Tests")]
    public class Exists : BaseInMemoryStorageTest
    {

        #region ID
        [Fact]
        public void ShouldExistByID()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var Item = data.First().ID;

            //act;
            var success = repository.Exists(Item);

            //assert
            Assert.True(success);

            Dispose();
        }
        [Fact]
        public void DoesNotExistByID()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var Item = Guid.NewGuid().ToString();

            //act;
            var success = repository.Exists(Item);

            //assert
            Assert.False(success);

            Dispose();
        }
        #endregion

        #region Object
        [Fact]
        public void ShouldExistByObject()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var Item = data.First();

            //act;
            var success = repository.Exists(Item);

            //assert
            Assert.True(success);

            Dispose();
        }
        [Fact]
        public void DoesNotExistByObject()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var Item = new MockDatabaseObject { ID = Guid.NewGuid().ToString() };

            //act;
            var success = repository.Exists(Item);

            //assert
            Assert.False(success);
        }
        #endregion

        #region Query
        [Fact]
        public void ShouldExistByQuery()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var Item = data.First().ID;

            //act;
            var success = repository.Exists(obj => obj.ID == Item);

            //assert
            Assert.True(success);
        }
        [Fact]
        public void DoesNotExistByQuery()
        {
            //arranage
            var data = MockObjects;
            var repository = CreateRepository();

            var Item = Guid.NewGuid().ToString();

            //act;
            var success = repository.Exists(obj => obj.ID == Item);

            //assert
            Assert.False(success);
        }
        #endregion
    }


}
