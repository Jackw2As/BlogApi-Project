using Domain.Base;

namespace Application.ViewModel
{
    public class PostDisplayModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<BaseTag> Tags { get; set; } = new ();
    }
}