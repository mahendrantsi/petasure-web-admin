using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartPay.Data;
using SmartPay.Data.DBEntities;
using SmartPay.Data.ExtendedDBEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SmartPay.Persistence.Repository
{
    public class UserOTPRepository : GenericRepository<UserOTP>
    {
        private readonly DbContext db;
        private readonly UserManager<DerivedIdentityUser> _userManager;

        public UserOTPRepository(DbContext dbContext, UserManager<DerivedIdentityUser> userManager) : base(dbContext)
        {
            this.db = dbContext;
            this._userManager = userManager;
        }
    }
}
