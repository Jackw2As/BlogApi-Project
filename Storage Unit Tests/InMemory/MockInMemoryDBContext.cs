﻿using BlogAPI.Storage.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Storage.InMemory
{
    public class MockInMemoryDBContext : InMemoryDBContext
    {
        public DbSet<MockDatabaseObject> MockDatabaseObject { get; set; }

        public MockInMemoryDBContext(DbContextOptions options) : base(options, false)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
