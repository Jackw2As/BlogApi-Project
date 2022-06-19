using Domain.Interface;
using Storage.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Service
{
    internal class SQLRepository<T> : IRepository<T> where T : BaseDatabaseModel
    {
        public void Create(T model)
        {
            throw new NotImplementedException();
        }

        public void Delete(T model)
        {
            throw new NotImplementedException();
        }

        public bool Exists(T model)
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

        public void Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
