﻿using Domain.Interface;
using Storage.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Service
{
    internal class InMemoryRepository<T> : IRepository<T> where T : BaseDatabaseModel
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
