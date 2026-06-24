using Microsoft.EntityFrameworkCore;
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
    public class SubscriptionRepository : GenericRepository<Subscriptions>
    {

        private readonly ProjectDbContext _db;

        public SubscriptionRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        //Get Pet List DB Work
        public List<SubscriptionViewModel> GetSubscriptionAll(Guid userId)
        {
            var user = _db.Users.FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                return new List<SubscriptionViewModel>();
            }
            var subscriptions = _db.Subscriptions.Where(a => a.CustomerId == user.ShopifyId).Select(s => new SubscriptionViewModel()
            {
                Id = s.Id,
                CustomerId = s.CustomerId,
                SubscriptionId = s.SubscriptionId,
                CancellationReason = s.CancellationReason,
                CancellationReasonComments = s.CancellationReasonComments,
                CancelledOn = s.CancelledOn,
                ChargeDelay = s.ChargeDelay,
                ChargeInvervalFrequency = s.ChargeInvervalFrequency,
                CreatedOn = s.CreatedOn,
                NextChargeScheduleOn = s.NextChargeScheduleOn,
                OrderInvervalFrequency = s.OrderInvervalFrequency,
                OrderInvervalUnit = s.OrderInvervalUnit,
                Price = s.Price,
                ProductTitle = s.ProductTitle,
                Quantity = s.Quantity,
                RechargeProductId = s.RechargeProductId,
                ShopifyProductId = s.ShopifyProductId,
                Status = s.Status,
                UpdatedOn = s.UpdatedOn,
                VariantTitle = s.VariantTitle

            }).ToList();

            return subscriptions;
        }

        public bool IsSubscriptionExist(long subscriptionId)
        {
            var data = _db.Subscriptions.Where(x => x.SubscriptionId == subscriptionId).FirstOrDefault();
            return data != null;
        }

        public Subscriptions GetById(long subscriptionId)
        {
            return _db.Subscriptions.FirstOrDefault(x => x.SubscriptionId == subscriptionId);
        }

        public async Task<bool> SaveSubscription(SubscriptionViewModel data)
        {
            // Validate input
            try
            {
                var model = new Subscriptions
                {   
                    CustomerId = data.CustomerId,
                    SubscriptionId = data.SubscriptionId,
                    CancellationReason = data.CancellationReason,
                    CancellationReasonComments = data.CancellationReasonComments,
                    CancelledOn = data.CancelledOn,
                    ChargeDelay = data.ChargeDelay,
                    ChargeInvervalFrequency = data.ChargeInvervalFrequency,
                    CreatedOn = data.CreatedOn,
                    NextChargeScheduleOn = data.NextChargeScheduleOn,
                    OrderInvervalFrequency = data.OrderInvervalFrequency,
                    OrderInvervalUnit = data.OrderInvervalUnit,
                    Price = data.Price,
                    ProductTitle = data.ProductTitle,
                    Quantity = data.Quantity,
                    Status = data.Status,
                    UpdatedOn = data.UpdatedOn,
                    VariantTitle = data.VariantTitle
                };

                // Add the pet model to the database
                await this._db.Set<Subscriptions>().AddAsync(model);

                // Save changes and determine success
                var affectedRows = await this._db.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }// Return true if at least one row was affected
        }



        public async Task<bool> UpdateSubscriptionOnCancelled(SubscriptionViewModel model)
        {
            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var dbEntity = _db.Subscriptions.FirstOrDefault(p => p.SubscriptionId == model.SubscriptionId);

            if (dbEntity == null)
            {
                throw new InvalidOperationException("Record not found for the given ID.");
            }

            // Update the relevant properties
            dbEntity.CancellationReason = model.CancellationReason;
            dbEntity.CancellationReasonComments = model.CancellationReasonComments;
            dbEntity.CancelledOn = model.CancelledOn;
            dbEntity.UpdatedOn = model.UpdatedOn;
            dbEntity.Status = model.Status;
            dbEntity.NextChargeScheduleOn = model.NextChargeScheduleOn;
            

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }

        public async Task<bool> UpdateSubscriptionOnActivation(SubscriptionViewModel model)
        {
            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var dbEntity = _db.Subscriptions.FirstOrDefault(p => p.SubscriptionId == model.SubscriptionId);

            if (dbEntity == null)
            {
                throw new InvalidOperationException("Record not found for the given ID.");
            }

            // Update the relevant properties
            dbEntity.CancellationReason = null;
            dbEntity.CancellationReasonComments = null;
            dbEntity.CancelledOn = null;
            dbEntity.NextChargeScheduleOn = model.NextChargeScheduleOn;
            dbEntity.UpdatedOn = model.UpdatedOn;
            dbEntity.Status = model.Status;

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }

        public async Task<bool> UpdateSubscriptionOnSkipped(SubscriptionViewModel model)
        {
            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var dbEntity = _db.Subscriptions.FirstOrDefault(p => p.SubscriptionId == model.SubscriptionId);

            if (dbEntity == null)
            {
                throw new InvalidOperationException("Record not found for the given ID.");
            }

            // Update the relevant properties
            dbEntity.NextChargeScheduleOn = model.NextChargeScheduleOn;
            dbEntity.UpdatedOn = model.UpdatedOn;
            dbEntity.Status = model.Status;

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }

        public async Task<bool> UpdateSubscriptionOnUnSkipped(SubscriptionViewModel model)
        {
            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var dbEntity =  _db.Subscriptions.FirstOrDefault(p => p.SubscriptionId == model.SubscriptionId);

            if (dbEntity == null)
            {
                throw new InvalidOperationException("Record not found for the given ID.");
            }

            // Update the relevant properties
            dbEntity.NextChargeScheduleOn = model.NextChargeScheduleOn;
            dbEntity.UpdatedOn = model.UpdatedOn;
            dbEntity.Status = model.Status;

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }

        public async Task<bool> UpdateSubscription(SubscriptionViewModel model)
        {
            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var dbEntity = _db.Subscriptions.FirstOrDefault(p => p.SubscriptionId == model.SubscriptionId);

            if (dbEntity == null)
            {
                throw new InvalidOperationException("Record not found for the given ID.");
            }

            // Update the relevant properties
            dbEntity.NextChargeScheduleOn = model.NextChargeScheduleOn;
            dbEntity.UpdatedOn = model.UpdatedOn;
            dbEntity.Status = model.Status;

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }
    }
}
