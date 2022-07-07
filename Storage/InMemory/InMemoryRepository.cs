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
            var DbSet = _dbContext.Set<T>();
            if (Exists(Id))
            {
                DbSet.Remove(GetByID(Id));
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Exists(Guid Id)
        {
            var DbSet = _dbContext.Set<T>();
            if (DbSet.Where(p => p.ID == Id) != null)
            {
                return true;
            }
            return false;
        }

        public bool Exists(T Model)
        {
            var DbSet = _dbContext.Set<T>();
            if (DbSet.First(p => p == Model) != null)
            {
                return true;
            }
            return false;
        }

        public bool Exists(Func<T, bool> query)
        {
            var DbSet = _dbContext.Set<T>();
            if (DbSet.Where(query) != null)
            {
                return true;
            }
            return false;
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
            if(Exists(model))
            {
                var dbSet = _dbContext.Set<T>();
                dbSet.Update(model);
                _dbContext.SaveChanges();
            }
            return false;
        }

        public bool Save(T model)
        {
            var dbSet = _dbContext.Set<T>();
            if (!Exists(model))
            {
                dbSet.Add(model);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
