using Domain.Base;

namespace Application.ViewModel
{
    public class BlogViewModel : BaseObject
    {
        public List<PostDisplayModel> DisplayModels { get; set; } = new ();
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
