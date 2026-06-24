using Microsoft.EntityFrameworkCore;
using Project.Core;
using Project.Data.DBEntities;
using Project.Models.Pets;
using Project.Models.Subscription;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public class InAppPurchaseRepository : GenericRepository<InAppPurchases>
    {

        private readonly ProjectDbContext _db;

        public InAppPurchaseRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public List<InAppPurchaseViewModel> GetInAppPurchases(Guid userId)
        {
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                return new List<InAppPurchaseViewModel>();
            }
            var inapps = _db.InAppPurchases.Where(a => a.AspnetuserId == userId && a.IsActive).Select(s => new InAppPurchaseViewModel()
            {
                Id = s.Id,
                CreatedOn = s.CreatedOn,
                ProductId = s.ProductId,
                Acknowledged = s.Acknowledged,
                PurchaseToken = s.PurchaseToken,
                TransactionId = s.TransactionId,
                TransactionReceipt = s.TransactionReceipt,
                TransactionDate = s.TransactionDate,
                ProductTitle = s.ProductTitle,
                ExpireDate = s.ExpireDate
            }).ToList();

            return inapps;
        }

        public (bool isvalid, string productTitle) IsCertificateValid(Guid userId, bool isSandBox)
        {
            var existing = _db.InAppPurchases.Where(a => a.AspnetuserId == userId && a.IsActive).OrderBy(o => o.ExpireDate).FirstOrDefault();

            var response = AppleReceiptValidator.ValidateReceiptAsync(existing.TransactionReceipt, isSandBox).GetAwaiter().GetResult();

            existing.IsActive = !(response.ExpireDate.Date < DateTime.Now.Date);
            existing.ExpireDate = response.ExpireDate;

            _db.InAppPurchases.Update(existing);
            _db.SaveChanges();

            return (existing.IsActive, existing.ProductTitle);
        }

        public async Task<bool> SaveInAppPurchase(InAppPurchaseInputViewModel data)
        {
            try
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(data.TransactionDate) / 1000);
                var model = new InAppPurchases
                {
                    AspnetuserId = data.AspnetuserId,
                    ProductId = data.ProductId,
                    Acknowledged = data.Acknowledged,
                    PurchaseToken = data.PurchaseToken,
                    TransactionDate = dateTimeOffset.DateTime,
                    TransactionId = data.TransactionId,
                    TransactionReceipt = data.TransactionReceipt,
                    CreatedOn = DateTime.Now,
                    CreatedBy = data.AspnetuserId,
                    IsActive = true,
                    ProductTitle = data.ProductTitle
                };

                await this._db.Set<InAppPurchases>().AddAsync(model);

                // Save changes and determine success
                var affectedRows = await this._db.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }// Return true if at least one row was affected
        }

        public async Task<bool> UpdateInAppPurchase(InAppPurchaseInputViewModel data)
        {
            try
            {
                var existing = _db.InAppPurchases.Where(x => x.ProductId == data.ProductId && x.AspnetuserId == data.AspnetuserId).FirstOrDefault();
                if (data != null)
                {
                    existing.ProductTitle = data.ProductTitle;
                    existing.TransactionReceipt = data.TransactionReceipt;
                    existing.IsActive = true;
                    existing.TransactionId = data.TransactionId;

                    var affectedRows = await this._db.SaveChangesAsync();
                    return affectedRows > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }// Return true if at least one row was affected
        }

        public bool IsExist(string productId, Guid userid)
        {
            var data = _db.InAppPurchases.Where(x => x.ProductId == productId && x.AspnetuserId == userid).FirstOrDefault();
            return data != null;
        }
    }
}
