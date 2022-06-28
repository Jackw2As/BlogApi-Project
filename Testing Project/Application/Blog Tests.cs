using System;
using System.Collections.Generic;

namespace Testing_Project.Unit_Tests
{
    public class BlogTests
    {
        [Theory]
        public async void GetBlog(Guid id)
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.GetAsync($"blog/get/{id}");
        }

        [Theory]
        public async void UpdateBlog(Guid id)
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.PostAsync($"blog/update/{id}");
        }

        [Theory]
        public async void DeleteBlog(Guid id)
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.DeleteAsync($"blog/delete/{id}");
        }
        [Fact]
        public async void CreateBlog()
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.PostAsync($"blog/create/");
        }
    }
}
