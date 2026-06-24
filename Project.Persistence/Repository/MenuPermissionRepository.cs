using Microsoft.EntityFrameworkCore;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Persistence.Repository
{
   public class MenuPermissionRepository : GenericRepository<MenuMaster>
    {
        private readonly ProjectDbContext db;

        public MenuPermissionRepository(ProjectDbContext dbContext)
           : base(dbContext)
        {
            this.db = dbContext;
        }
            //public List<MenuListResult> GetMenuList(params object[] parameters)
            //{
            //    List<MenuListResult> viewModel = db.MenuListResult.FromSqlRaw($"sp_GetMenuList {parameters[0]}").ToList();
            //    return viewModel;
            //}
    }
}
