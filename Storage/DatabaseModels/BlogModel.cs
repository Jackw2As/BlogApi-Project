using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.DatabaseModels
{
    public class BlogModel : BaseDatabaseModel
    {
        List<PostModel> PostModels { get; set; }

        public string Name { get; set; }
    }
}
