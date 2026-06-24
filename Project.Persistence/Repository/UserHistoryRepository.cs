using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public class UserHistoryRepository : GenericRepository<UserHistory>
    {
        private readonly ProjectDbContext _db;
        public UserHistoryRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            this._db = dbContext;
        }
    }
}
