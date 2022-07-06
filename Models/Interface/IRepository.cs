using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interface
{
    public interface IRepository<T> where T : BaseObject
    {
        bool Create(T model);
        T GetByID(Guid Id);
        bool Update(T model);
        bool Delete(Guid Id);
        IEnumerable<T> GetByQuery(Func<T, bool> query);
        bool Exists(Guid Id);
    }
}