using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Project.Data.ExtendedDBEntities;
using System.Threading.Tasks;
using System.Linq;
using Project.Core.Enum;
using Project.Core.Extension;
using System.Net;
using ServiceStack;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Models.CommonModel;
using System.Data;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Project.Persistence.Repository
{
    public class UserAccountRepository : GenericRepository<UserDetailModel>
    {
        private readonly ProjectDbContext _db;
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private readonly SignInManager<DerivedIdentityUser> _signInManager;
        private readonly string[] role = { EnumRole.User.ToString() };
        public UserAccountRepository(ProjectDbContext dbContext, UserManager<DerivedIdentityUser> userManager, SignInManager<DerivedIdentityUser> signInManager)
           : base(dbContext)
        {
            this._db = dbContext;
            this._userManager = userManager;
            _signInManager = signInManager;

        }

        public virtual IEnumerable<UserDetailModel> GetUsers(
            Expression<Func<DerivedIdentityUser, bool>> filter = null,
            Func<IQueryable<DerivedIdentityUser>, IOrderedQueryable<DerivedIdentityUser>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<DerivedIdentityUser> query = _db.Users.Where(a => a.IsActive == true && a.IsDeleted == false);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return (from u in query.ToList()
                    join r in _db.UserRoles on u.Id equals r.UserId
                    join rName in _db.Roles on r.RoleId equals rName.Id /*&& role.Contains(includeProperty)*/
                    where rName.Name == EnumRole.User.ToString() || rName.Name == EnumRole.SecondayUser.ToString()
                    select new UserDetailModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        IsActive = u.IsActive,
                        Email = u.Email,
                        IsDeleted = u.IsDeleted,
                        IsDeviceConnected = u.IsDeviceConnected,
                        CreatedOn = u.CreatedOn,
                        PhoneNumber = u.PhoneNumber,
                        role = rName.Name == EnumRole.SecondayUser.ToString() ? "Secondary User" : rName.Name,
                        ShopifyId = u.ShopifyId,
                        UserType = u.UserType
                    }).ToList();
        }


        public async Task<UserRegister> CreateAccount(UserRegister model)
        {
            var userIdentity = new DerivedIdentityUser()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = model.CreatedOn,
                ParentUserID = model.ParentUserID,
                ShopifyId = model.ShopifyId,
                ShopifyResponse = model.ShopifyResponse,
                UserType = model.UserType,
                Address = model.Address
            };

            using var transaction = this._db.Database.BeginTransaction();
            try
            {
                var result = await this._userManager.CreateAsync(userIdentity, model.Password);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(model.Role))
                        await this._userManager.AddToRoleAsync(userIdentity, EnumRole.User.ToString());
                    else
                        await this._userManager.AddToRoleAsync(userIdentity, model.Role);

                    await this._db.SaveChangesAsync();
                    transaction.Commit();
                    model.Id = userIdentity.Id;
                    model.IsSuccess = true;
                }
                else
                {
                    Dictionary<int, string> errors = new Dictionary<int, string>();
                    var i = 1;
                    foreach (var error in result.Errors)
                    {
                        errors.Add(i, error.Description);
                        i++;
                    }
                    model.error = errors;
                    model.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return model;
        }

        public async Task<bool> isMobileNumberExists(string MobileNumber)
        {
            var user = await this._db.Set<DerivedIdentityUser>().FirstOrDefaultAsync(x => x.PhoneNumber == MobileNumber);

            if (user != null)
                return true;
            else
                return false;
        }


        public async Task<UserRegister> CreateUser(UserRegister model)
        {

            var userIdentity = new DerivedIdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedOn = model.CreatedOn,
                IsActive = true,
                IsDeleted = false,
            };
            using var transaction = this._db.Database.BeginTransaction();
            try
            {
                var result = await this._userManager.CreateAsync(userIdentity, model.Password);
                if (result.Succeeded)
                {
                    await this._userManager.AddToRoleAsync(userIdentity, model.Role);

                    await this._db.Set<UserProfile>().AddAsync(new UserProfile()
                    {
                        UserId = userIdentity.Id
                    });

                    await this._db.SaveChangesAsync();
                    transaction.Commit();
                    model.Id = userIdentity.Id;
                    model.IsSuccess = true;
                }
                else
                {
                    Dictionary<int, string> errors = new Dictionary<int, string>();
                    var i = 1;
                    foreach (var error in result.Errors)
                    {
                        errors.Add(i, error.Description);
                        i++;
                    }
                    model.error = errors;
                    model.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return model;
        }

        public async Task<DerivedIdentityUser> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.IsActive = false;
                    user.IsDeleted = true;
                }
                await this._db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DerivedIdentityUser> DeleteUserPermanent(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
                await this._db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserRegister> UpdateUser(UserRegister model)
        {
            using var transaction = this._db.Database.BeginTransaction();
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (user != null)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.IsActive = model.Active;

                    var response = await _userManager.UpdateAsync(user);
                    if (response.Succeeded)
                    {
                        var userProfile = this._db.Set<UserProfile>().Where(x => x.UserId == model.UserID).FirstOrDefault();
                        if (userProfile != null)
                        {
                            if (!string.IsNullOrEmpty(model.UserImage))
                            {
                            }
                        }
                        await this._db.SaveChangesAsync();
                        transaction.Commit();
                        model.IsSuccess = true;
                        return model;
                    }
                    else
                    {
                        Dictionary<int, string> errors = new Dictionary<int, string>();
                        var i = 1;
                        foreach (var error in response.Errors)
                        {
                            errors.Add(i, error.Description);
                            i++;
                        }
                        model.error = errors;
                        model.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }

            return model;
        }
        //public async Task<bool> UpdateKYBStatus(long userID, EnumKYBStatus status)
        //{
        //    try
        //    {
        //        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userID);
        //        if (user != null)
        //        {
        //            user.KYBStatus = status;
        //        }

        //        await this._db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> UpdateKYCStatus(long userID, EnumKycStatus status)
        //{
        //    try
        //    {
        //        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userID);
        //        if (user != null)
        //        {
        //            user.IsKyc = status;
        //        }

        //        await this._db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public int GetTotalUsers()
        {
            int totalUsers = 0;
            IQueryable<DerivedIdentityUser> query = _db.Users.Where(a => a.IsActive == true && a.IsDeleted == false);
            totalUsers = (from u in query.ToList()
                      join r in _db.UserRoles on u.Id equals r.UserId
                      join rName in _db.Roles on r.RoleId equals rName.Id
                      where rName.Name.ToLower() == EnumRole.User.ToString().ToLower() || rName.Name.ToLower() == EnumRole.SecondayUser.ToString().ToLower()
                          select new { u.Id }).Count();
            
            return totalUsers;
        }
        public int GetMonthlyNewUsers()
        {
            int totalUsers = 0;
            IQueryable<DerivedIdentityUser> query = _db.Users.Where(a => a.IsActive == true && a.IsDeleted == false 
                                                            && (a.CreatedOn >= DateTime.UtcNow.AddDays(-30) && a.CreatedOn <= DateTime.UtcNow));
            totalUsers = (from u in query.ToList()
                          join r in _db.UserRoles on u.Id equals r.UserId
                          join rName in _db.Roles on r.RoleId equals rName.Id
                          where rName.Name.ToLower() == EnumRole.User.ToString().ToLower() || rName.Name.ToLower() == EnumRole.SecondayUser.ToString().ToLower()
                          select new { u.Id }).Count();
            return totalUsers;
        }

    }
}
