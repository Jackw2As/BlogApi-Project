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
        private readonly DbSet<T> DbSet;
        public InMemoryRepository(T[] data, DbContextOptions options)
        {
            _dbContext = new(options) { DataObjectsSeedData = data };
            DbSet = _dbContext.Set<T>();
            _dbContext.Database.EnsureCreated();

            _dbContext.SaveChanges();
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
            var model = DbSet.Find(Id);
            if (model == null)
            {
                throw new ArgumentException($"{Id} isn't a valid ID. Entity couldn't be found!");
            }
            return model;
        }

        public IEnumerable<T> GetByQuery(Func<T, bool> query)
        {
            var collection = DbSet.Where(query);
            return collection;
        }

        public bool Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
