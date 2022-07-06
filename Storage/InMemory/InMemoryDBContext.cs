using BlogAPI.Storage.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Storage.InMemory
{
    public class InMemoryDBContext : DbContext
    {
        //Note: For each DBSet create an Array create a public SeedData Class
        public DbSet<MockDatabaseObject> MockDatabaseObject { get; set; }

        public MockDatabaseObject[]? MockDatabaseObjectSeedData { get; init; }

        public InMemoryDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if(MockDatabaseObjectSeedData != null)
            modelBuilder.Entity<MockDatabaseObject>().HasData(MockDatabaseObjectSeedData);
        }
    }
}
