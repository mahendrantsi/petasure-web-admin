using Microsoft.EntityFrameworkCore;
using ServiceStack;
using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Models.ProfileModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public class RolePremissionRepository : GenericRepository<StaffRole>
    {
        private readonly ProjectDbContext _db;
        public RolePremissionRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            this._db = dbContext;
        }

        //public async Task<StaffRole> AddStaffRole(StaffRoleViewModel model, long CreatedBy)
        //{
        //    try
        //    {
        //        var staffRole = await this._db.Set<StaffRole>().AddAsync(new StaffRole
        //        {
        //            BusinessID = model.BusinessId.Value,
        //            Name = model.RoleName,
        //            CreatedBy = CreatedBy,
        //            CreatedOn = DateTime.UtcNow,
        //            IsActive = model.IsActive
        //        });
        //        await this._db.SaveChangesAsync();
        //        return staffRole.Entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async Task<StaffRole> UpdateStaffRole(StaffRoleViewModel model, long CreatedBy)
        //{
        //    var staffRole = await this._db.Set<StaffRole>().FirstOrDefaultAsync(u => u.Id == model.Id);
        //    staffRole.Name = model.RoleName;
        //    staffRole.IsActive = model.IsActive;
        //    staffRole.UpdatedBy = CreatedBy;
        //    staffRole.UpdatedOn = DateTime.UtcNow; 
        //    var response = this._db.Set<StaffRole>().Update(staffRole);

        //    await this._db.SaveChangesAsync();
        //    return response.Entity;
        //}
    }
}
