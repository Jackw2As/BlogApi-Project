using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interface
{
    public interface IRepository<T> where T : BaseModel
    {
        void Create(T model);
        T Read(Guid Id);
        void Update(T model);
        void Delete(T model);

        IEnumerable<T> ReadMultiple();

        bool Exists(T model);
    }
}