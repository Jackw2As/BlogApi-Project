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
        public InMemoryRepository(DbContextOptions options, MockDatabaseObject[]? mockSeedData = null)
        {
            _dbContext = new(options)
            {
                MockDatabaseObjectSeedData = mockSeedData            
            };
            DbSet = _dbContext.Set<T>();
            _dbContext.Database.EnsureCreated();

            _dbContext.SaveChanges();
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
