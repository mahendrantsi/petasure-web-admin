using Project.Data.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enum;
using System.ComponentModel.DataAnnotations;
using ServiceStack;
using Project.Models.ProfileModel;

namespace Project.Persistence.Repository
{
    public class StaffRolePermissionRepository : GenericRepository<StaffRolePermission>
    {
        private readonly ProjectDbContext _db;
        public StaffRolePermissionRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            this._db = dbContext;
        }

        //public async Task<List<StaffRolePermission>> AddStaffRolePermission(List<RoleModuleViewModel> model, long staffRoleID, long createdBy)
        //{

        //    var details = await this.GetStaffRolePermission(staffRoleID);
        //    this._db.Set<StaffRolePermission>().RemoveRange(details);
        //    var permission = model.Where(x=>x.IsSelected)
        //                          .Select(x => new StaffRolePermission()
        //                          {
        //                              ModuleID = x.ModuleID,
        //                              CreatedBy = createdBy,
        //                              StaffRoleID = staffRoleID
        //                          }).ToList();
        //    await this._db.Set<StaffRolePermission>().AddRangeAsync(permission);
        //    await this._db.SaveChangesAsync();
        //    return permission;
        //}



        public async Task<List<StaffRolePermission>> GetStaffRolePermission(long staffRoleID)=>this._db.Set<StaffRolePermission>().Where(x => x.StaffRoleID == staffRoleID).ToList();
    }
}
