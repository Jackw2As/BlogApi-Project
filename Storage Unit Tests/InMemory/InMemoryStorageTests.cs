using BlogAPI.Storage.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.InMemory
{
    public class InMemoryStorageTests
    {
        [Fact]
        public void ShouldGetDataObjectwithID()
        {
            //Arrange
            DataObject[] data = { new() { ID = Guid.NewGuid() } };

            InMemoryRepository<DataObject> repository = new(data);


            //Act
            var dataInDatabase = repository.Read(data[0].ID);

            //Assert
            Assert.NotNull(data);
            Assert.Equal(data[0].ID, dataInDatabase.ID);
        }
        [Fact]
        public void ShouldReturnArgumentException()
        {
            DataObject[] data = { new() { ID = Guid.NewGuid() } };
            InMemoryRepository<DataObject> repository = new(data);

            Assert.ThrowsAny<ArgumentException>(() => repository.Read(Guid.NewGuid()));
        }
    }
}
