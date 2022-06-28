using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interface
{
    public interface IRepository<T> where T : BaseModel
    {
        bool Create(T model);
        T Read(Guid Id);
        bool Update(T model);
        bool Delete(Guid Id);

        IEnumerable<T> ReadMultiple();
        bool Exists(Guid Id);
    }
}