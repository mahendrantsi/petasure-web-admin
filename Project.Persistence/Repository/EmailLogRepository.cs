using Microsoft.EntityFrameworkCore;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Persistence.Repository
{
   public class EmailLogRepository : GenericRepository<EmailLog>
    {
        private readonly ProjectDbContext db;

        public EmailLogRepository(ProjectDbContext dbContext)
           : base(dbContext)
        {
            this.db = dbContext;
        }
    }
}
