using BlogAPI.Storage.DatabaseModels;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Storage.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : DataObject
    {
        private readonly InMemoryDBContext _dbContext;
        public InMemoryRepository(T[] data, DbContextOptions options)
        {
            _dbContext = new(options) { DataObjectsSeedData = data };
        }

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

        public T GetByID(Guid Id)
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
