using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyComment : BaseObject
    {
        public string Content { get; set; }
        public DateTime DateModfied { get; }
        public ModifyComment(GetComment getComment)
        {
            ID = getComment.ID;
            Content = getComment.Content;
            DateModfied = DateTime.UtcNow;
        }
    }
}
