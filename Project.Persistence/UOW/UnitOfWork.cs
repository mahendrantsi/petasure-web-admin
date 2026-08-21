namespace Project.Persistence.UOW
{
    using Project.Data;
    using Project.Persistence.Repository;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Project.Persistence.IRepository;
    using Microsoft.AspNetCore.Identity;
    using Project.Data.ExtendedDBEntities;
    using Project.Data.DBEntities;
    using AutoMapper;
    using Microsoft.Extensions.Logging;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDbContext context;
        private readonly UserManager<DerivedIdentityUser> _userManager;
        private readonly SignInManager<DerivedIdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UnitOfWork> _logger;
        public UnitOfWork(ProjectDbContext dbContext, UserManager<DerivedIdentityUser> userManager, SignInManager<DerivedIdentityUser> signInManager, IMapper mapper, ILogger<UnitOfWork> logger)
        {
            this.context = dbContext;
            this._userManager = userManager;
            _signInManager = signInManager;
            this._mapper = mapper;
            this._logger = logger;
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            IGenericRepository<T> repo = new GenericRepository<T>(context);
            return repo;
        }

        public ProjectDbContext Instance { get { return this.context; } }

        public bool Save()
        {
            bool returnValue = true;
            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Previously swallowed with no logging at all — every caller of Save()/
                // SaveChanges()/SaveChangesAsync() across the app ignores the bool return
                // value, so a failed save (e.g. a constraint violation) was completely
                // invisible: no exception, no log entry, just a silent no-op rollback.
                _logger.LogError(ex, "UnitOfWork.Save failed — changes were not persisted.");
                returnValue = false;
            }

            return returnValue;
        }

        public bool SaveChanges()
        {
            bool returnValue = true;
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    this.Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UnitOfWork.SaveChanges failed — transaction rolled back.");
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }

            return returnValue;
        }

        public async Task<bool> SaveChangesAsync()
        {
            bool returnValue = true;
            using (var dbContextTransaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    await this.Context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UnitOfWork.SaveChangesAsync failed — transaction rolled back.");
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }

            return returnValue;
        }


        #region Public Properties

        private UserAccountRepository userAccountRepository;
        public UserAccountRepository UserAccountRepository => this.userAccountRepository ?? (this.userAccountRepository = new UserAccountRepository(this.Context, this._userManager,this._signInManager));

        private PetRepository petRepository;
        public PetRepository PetRepository => this.petRepository ?? (this.petRepository = new PetRepository(this.Context));

        
        private SubscriptionRepository subscriptionRepository;
        public SubscriptionRepository SubscriptionRepository => this.subscriptionRepository ?? (this.subscriptionRepository = new SubscriptionRepository(this.Context));

        private InAppPurchaseRepository inAppPurchaseRepository;
        public InAppPurchaseRepository InAppPurchaseRepository => this.inAppPurchaseRepository ?? (this.inAppPurchaseRepository = new InAppPurchaseRepository(this.Context));




        private MissingPetRepository missingPetRepository;
        public MissingPetRepository MissingPetRepository => this.missingPetRepository ?? (this.missingPetRepository = new MissingPetRepository(this.Context));


        //private UserOTPRepository userOTPRepository;
        //public UserOTPRepository UserOTPRepository => this.userOTPRepository ?? (this.userOTPRepository = new UserOTPRepository(this.Context, this._userManager));

        private UserProfileRepository userProfileRepository;
        public UserProfileRepository UserProfileRepository => this.userProfileRepository ?? (this.userProfileRepository = new UserProfileRepository(this.Context));


        private EmailLogRepository emailLogRepository;
        public EmailLogRepository EmailLogRepository => this.emailLogRepository ?? (this.emailLogRepository = new EmailLogRepository(this.context));

        private IntegrationRepository integrationRepository;
        public IntegrationRepository IntegrationRepository => this.integrationRepository ?? (this.integrationRepository = new IntegrationRepository(this.context,this._mapper));


      


        private RolePremissionRepository rolePremissionRepository;
        public RolePremissionRepository RolePremissionRepository => this.rolePremissionRepository ?? (this.rolePremissionRepository = new RolePremissionRepository(this.context));

       

        private StaffRolePermissionRepository staffRolePermissionRepository;
        public StaffRolePermissionRepository StaffRolePermissionRepository => this.staffRolePermissionRepository ?? (this.staffRolePermissionRepository = new StaffRolePermissionRepository(this.context));

      
        
        private UserHistoryRepository userHistoryRepository;
        public UserHistoryRepository UserHistoryRepository => this.userHistoryRepository ??
          (this.userHistoryRepository = new UserHistoryRepository(this.context));

            public UserManagementRepository<T> UserManagementRepository<T>() where T : class
            {
                UserManagementRepository<T> userRepo = new UserManagementRepository<T>(context);
                return userRepo;
            }

 

        protected ProjectDbContext Context => this.context;

       



        #endregion

        #region IDisposable Support  
        private bool _disposedValue = false; //  To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                //  dispose managed state (managed objects).
            }

            //  free unmanaged resources (unmanaged objects) and override a finalizer below.
            //  set large fields to null.

            _disposedValue = true;
        }

        //  override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.  
        //  ~UnitOfWork() {  
        //    //  Do not change this code. Put cleanup code in Dispose(bool disposing) above.  
        //    Dispose(false);  
        //  }  

        //  This code added to correctly implement the disposable pattern.  
        public void Dispose()
        {
            //  Do not change this code. Put cleanup code in Dispose(bool disposing) above.  
            Dispose(true);
            //  uncomment the following line if the finalizer is overridden above.  
            //  GC.SuppressFinalize(this);  
        }
        #endregion

    }
}