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
            //Check to see if data exists before deleting.
            if (!Exists(id)) return false;
            
            dbSet.Remove(GetByID(id));
            DbContext.SaveChanges();
            
            //Validation check to ensure data is deleted. Expect null return. 
            var entity = dbSet.Find(id);
            return entity == null;
        }

        public bool Exists(string id)
        {
            return Exists(p => p.ID == id);
        }

        public bool Exists(T model)
        {
            return Exists(dataObject => Equals(dataObject, model));
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
