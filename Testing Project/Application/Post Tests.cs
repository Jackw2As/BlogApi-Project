using Microsoft.AspNetCore.Mvc.Testing;
using System;

namespace Testing_Project.Unit_Tests
{
    public class PostTests : IClassFixture<WebApplicationFactory<Program>>
    {
        [Theory]
        public async void GetPost(Guid id)
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.GetAsync($"blog/get/{id}");
        }

        [Theory]
        public async void UpdatePost(Guid id)
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.PostAsync($"blog/update/{id}");
        }

        [Theory]
        public async void DeletePost(Guid id)
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.DeleteAsync($"blog/delete/{id}");
        }
        [Fact]
        public async void CreatePost()
        {
            var results = TestWebServer.Create();

            var client = results.Item2;

            await client.PostAsync($"blog/create/");
        }
    }
}
