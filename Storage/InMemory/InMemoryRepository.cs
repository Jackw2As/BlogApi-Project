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
        //private readonly DbSet<T> DbSet;
        public InMemoryRepository(InMemoryDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(T Model)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<T, bool> query)
        {
            throw new NotImplementedException();
        }

        public T GetByID(Guid Id)
        {
            var DbSet = _dbContext.Set<T>();
            
            var model = DbSet.Find(Id);
            if (model == null)
            {
                throw new ArgumentException($"{Id} isn't a valid ID. Entity couldn't be found!");
            }
            return model;
        }

        public IEnumerable<T> GetByQuery(Func<T, bool> query)
        {
            var collection = _dbContext.Set<T>().Where(query);
            return collection;
        }

        public bool Modify(T model)
        {
            throw new NotImplementedException();
        }

        public bool Save(T model)
        {
            throw new NotImplementedException();
        }
    }
}
