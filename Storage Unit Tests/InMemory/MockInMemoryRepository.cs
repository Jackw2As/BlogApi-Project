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
    public class MockInMemoryRepository : InMemoryRepository<MockDatabaseObject>
    {
        public MockInMemoryRepository(MockInMemoryDBContext dbContext) : base(dbContext, false)
        {
        }
    }
}
