using Domain.Base;

namespace BlogAPI.Application.ApiModels
{
    public class ModifyComment : BaseObject
    {
        public string Content { get; set; }

        public ModifyComment()
        {
            Content = String.Empty;
        }
        public ModifyComment(GetComment getComment)
        {
            ID = getComment.ID;
            Content = getComment.Content;
        }
        public ModifyComment(string content)
        {
            Content = content;
        }
    }
}
