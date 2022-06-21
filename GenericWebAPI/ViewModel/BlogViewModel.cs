using Domain.Base;

namespace Application.ViewModel
{
    public class BlogViewModel : BaseModel
    {
        public List<PostDisplayModel> DisplayModels { get; set; } = new ();
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
