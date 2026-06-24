using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
   public class UserManagementRepository<T> : GenericRepository<T> where T : class
    {
        private readonly ProjectDbContext db;

        public UserManagementRepository(ProjectDbContext dbContext)
           : base(dbContext)
        {
            this.db = dbContext;

        }
        //public List<UserListResult> GetUserList(params object[] parameters)
        //{
        //    List<UserListResult> viewModel = db.UserListResult.FromSqlRaw($"sp_GetUserDetail {parameters[0]},'{parameters[1]}'").ToList();
        //    return viewModel;
        //}
    }
}
