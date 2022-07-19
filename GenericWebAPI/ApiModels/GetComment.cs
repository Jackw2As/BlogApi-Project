using BlogAPI.Storage.DatabaseModels;
using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class GetComment : BaseObject
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public Post Post { get; set; }
    }
}
