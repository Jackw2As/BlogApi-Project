using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.InMemory.GuidToString
{
    public class GuidToStringTests
    {
        [Fact]
        public void Test1Value()
        {
            //Arrange
            string item1 = "BAD";

            List<string> itemList = new();
            itemList.Add(item1);

            var test = new EnumerableGuidToString();

            //Act
            var result1 = test.ConvertToProvider(itemList);
            var result2 = test.ConvertFromProvider(result1);

            //Assert
            Assert.Equal(itemList, result2);
        }
        [Fact]
        public void Test5Values()
        {
            //Arrange
            string item1 = "BAD";
            string item2 = "BAD";
            string item3 = "BAD";
            string item4 = "BAD";
            string item5 = "BAD";

            List<string> itemList = new();
            itemList.Add(item1);
            itemList.Add(item2);
            itemList.Add(item3);
            itemList.Add(item4);
            itemList.Add(item5);

            var test = new EnumerableGuidToString();

            //Act
            string result1 = (string)test.ConvertToProvider(itemList);
            List<string> result2 = (List<string>)test.ConvertFromProvider(result1);

            //Assert
            Assert.Equal(itemList, result2);
        }
        [Fact]
        public void Test50Values()
        {
            //Arrange
            string item1 = "BAD";

            List<string> itemList = new();

            for (int i = 0; i < 50; i++)
            {
                itemList.Add(Guid.NewGuid().ToString());
            }

            var test = new EnumerableGuidToString();

            //Act
            var result1 = test.ConvertToProvider(itemList);
            var result2 = test.ConvertFromProvider(result1);

            //Assert
            Assert.Equal(itemList, result2);
        }
        [Fact]
        public void Test0Values()
        {
            //Arrange
            string item1 = string.Empty;

            List<string> itemList = new();
            itemList.Add(item1);

            var test = new EnumerableGuidToString();

            //Act
            var result1 = test.ConvertToProvider(itemList);
            var result2 = test.ConvertFromProvider(result1);

            //Assert
            Assert.Equal(itemList, result2);
        }
    }
}
