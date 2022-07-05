using BlogAPI.Storage.DatabaseModels;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.Service
{
    public class SQLRepository<T> : IRepository<T> where T : BaseDatabaseModel
    {
        public bool Create(T model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid Id)
        {
            throw new NotImplementedException();
        }

        public T Read(Guid Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ReadMultiple()
        {
            throw new NotImplementedException();
        }

        public bool Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
