using System.Collections.ObjectModel;
using BlogAPI.Storage.DatabaseModels;
using Domain.Interface;

namespace BlogAPI.Storage.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : DataObject
    {
        protected readonly InMemoryDbContext DbContext;
        public InMemoryRepository(InMemoryDbContext dbContext, bool seedDatabase = true)
        {
            DbContext = dbContext;
            if(seedDatabase) SeedData.SeedDatabase(DbContext);
        }

        public bool Delete(string id)
        {
            var dbSet = DbContext.Set<T>();
            //Don't attempt Deleting unless it exists in the database.
            if (!Exists(id)) return false;
            
            dbSet.Remove(GetByID(id));
            DbContext.SaveChanges();
            
            //Validation check to ensure data is deleted. Expected result is null. 
            var entity = dbSet.Find(id);
            if (entity == null) return true;
            return false;
        }

        public bool Exists(string id)
        {
            var dbSet = DbContext.Set<T>();

            if (dbSet.Any(p => p.ID == id))
            {
                return true;
            }
            return false;
        }

        public bool Exists(T model)
        {
            var dbSet = DbContext.Set<T>();
            return dbSet.Any(dataObject => dataObject == model);
        }

        public bool Exists(Func<T, bool> query)
        {
            var dbSet = DbContext.Set<T>();
            return dbSet.Any(query);
        }

        public T GetByID(string id)
        {
            var model = DbContext.Set<T>().Find(id);
            if (model == null)
            {
                throw new ArgumentException($"{id} isn't a valid ID. Entity couldn't be found!");
            }
            return model;
        }

        public IEnumerable<T> GetByQuery(Func<T, bool> query)
        {
            return DbContext.Set<T>().Where(query);
        }

        public bool Modify(T model)
        {
            //Check to make sure the object exists in database before trying to update.
            if (!Exists(model)) return false;
            DbContext.Set<T>().Update(model);
            DbContext.SaveChanges();
            return true;
        }

        public bool Save(T model)
        {
            //We only save if the object does not exist in the database. Otherwise we need to call Modify
            if (Exists(model)) return false;
            DbContext.Set<T>().Add(model);
            DbContext.SaveChanges();
            return true;
        }
    }
}
