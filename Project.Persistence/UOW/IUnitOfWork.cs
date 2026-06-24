using Project.Data.DBEntities;
using Project.Persistence.IRepository;
using Project.Persistence.Repository;
using System;
using System.Threading.Tasks;

namespace Project.Persistence.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        bool SaveChanges();
        Task<bool> SaveChangesAsync();
        bool Save();
        public ProjectDbContext Instance { get; }
        //public ProjectDbContext Instance { get; }
        UserAccountRepository UserAccountRepository { get; }
        UserProfileRepository UserProfileRepository { get; }

        EmailLogRepository EmailLogRepository { get; }

        UserManagementRepository<T> UserManagementRepository<T>() where T : class;
      
        RolePremissionRepository RolePremissionRepository { get; }
        StaffRolePermissionRepository StaffRolePermissionRepository { get; }
      
        IGenericRepository<T> GenericRepository<T>() where T : class;
        UserHistoryRepository UserHistoryRepository { get; }
        IntegrationRepository IntegrationRepository { get; }

        PetRepository PetRepository { get; }
        SubscriptionRepository SubscriptionRepository { get; }
        InAppPurchaseRepository InAppPurchaseRepository { get; }

    

        MissingPetRepository MissingPetRepository { get; }
    }
}
