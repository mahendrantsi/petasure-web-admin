using Project.Data;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public class UserProfileRepository : GenericRepository<UserProfile>
    { 
        private readonly ProjectDbContext _db;
        //private readonly UserManager<DerivedIdentityUser> _userManager;

        public UserProfileRepository(ProjectDbContext dbContext) : base(dbContext)//,UserManager<DerivedIdentityUser> userManager) : base(dbContext)
        {
            this._db = dbContext;
            //this._userManager = userManager;
        }

        public UserProfile GetUserProfile(Guid userId) 
        {
            return _db.UserProfile.First(x => x.UserId == userId);
        }

        public void getUsersList()
        {

        }

        public async Task<UserRegister> SaveProfile(UserRegister model)
        {
            await this._db.Set<UserProfile>().AddAsync(new UserProfile()
            {
                //ModifiedBy = model.ModifiedBy,
            });

            await this._db.SaveChangesAsync();
          
            model.IsSuccess = true;
            return model;

        }


       
    }
}
