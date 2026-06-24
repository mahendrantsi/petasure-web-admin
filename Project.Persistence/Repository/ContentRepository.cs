using Project.Data.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public class ContentRepository : GenericRepository<ContentMaster>
    {
        private readonly DbContext db;
        public ContentRepository(DbContext dbContext) : base(dbContext)
        {
            this.db = dbContext;
        }
    }
}
