using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;

namespace Testing_Project
{
    public static class TestWebServer
    {
        public static (TestServer, HttpClient) Create()
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            var server = application.Server;

            return (server, client);
        }
    }
}
