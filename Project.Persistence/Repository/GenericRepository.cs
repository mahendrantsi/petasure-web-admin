namespace Project.Persistence.Repository
{
    using System;
    using System.Collections.Generic;
  
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Project.Data;
    using Project.Persistence.IRepository;
    using Microsoft.EntityFrameworkCore;
    using ServiceStack.Script;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> Entities;
        private readonly DbContext _dbContext;

        // / <summary>  
        // / Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.  
        // / Note that here I've stored Context.Set<TEntity>() in the constructor and store it in a private field like _entities.   
        // / This way, the implementation  of our methods would be cleaner:        // /   
        // / _entities.ToList();  
        // / _entities.Where();  
        // / _entities.SingleOrDefault();  
        // / </summary>  
        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            Entities = _dbContext.Set<TEntity>();
        }


        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = Entities;

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
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        // / <summary>  
        // / Gets the specified identifier.  
        // / </summary>  
        // / <param name="id">The identifier.</param>  
        // / <returns></returns>  
        public virtual TEntity GetById(int id)
        {
            //  Here we are working with a DbContext, not specific DbContext.   
            //  So we don't have DbSets we need to use the generic Set() method to access them.  
            return this.Entities.Find(id);
        }
        public virtual TEntity GetById(Guid id)
        {
            // Here we are working with a DbContext, not specific DbContext.   
            // So we don't have DbSets we need to use the generic Set() method to access them.  
            return this.Entities.Find(id);
        }
        public async virtual Task<TEntity> GetByIdAsync(int id)
        {
            //  Here we are working with a DbContext, not specific DbContext.   
            //  So we don't have DbSets we need to use the generic Set() method to access them.  
            return await this.Entities.FindAsync(id);
        }
        public async virtual Task<TEntity> GetByIdAsync(Guid id)
        {
            // Here we are working with a DbContext, not specific DbContext.   
            // So we don't have DbSets we need to use the generic Set() method to access them.  
            return await this.Entities.FindAsync(id);
        }
        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return this.Entities;
            }
        }

        // / <summary>  
        // / Gets all.  
        // / </summary>  
        // / <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            return Entities.ToList();
        }

        // / <summary>  
        // / Finds the specified predicate.  
        // / </summary>  
        // / <param name="predicate">The predicate.</param>  
        // / <returns></returns>  
        public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate);
        }

        // / <summary>  
        // / Singles the or default.  
        // / </summary>  
        // / <param name="predicate">The predicate.</param>  
        // / <returns></returns>  
        public TEntity SingleOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).SingleOrDefault();
        }

        // / <summary>  
        // / First the or default.  
        // / </summary>  
        // / <returns></returns>  
        public TEntity FirstOrDefault()
        {
            return Entities.SingleOrDefault();
        }

        // / <summary>  
        // / Adds the specified entity.  
        // / </summary>  
        // / <param name="entity">The entity.</param>  
        public void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        public async virtual Task AddAsync(TEntity entity)
        {
            Entities.AddAsync(entity);
        }
         
        // / <summary>  
        // / Adds the range.  
        // / </summary>  
        // / <param name="entities">The entities.</param>  
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        // / <summary>  
        // / Removes the specified entity.  
        // / </summary>  
        // / <param name="entity">The entity.</param>  
        public void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        // / <summary>  
        // / Removes the range.  
        // / </summary>  
        // / <param name="entities">The entities.</param>  
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities); 
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
        }


        // / <summary>  
        // / Removes the Entity  
        // / </summary>  
        // / <param name="entityToDelete"></param>  
        public virtual void RemoveEntity(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                Entities.Attach(entityToDelete);
            }
            Entities.Remove(entityToDelete);

        }

        // / <summary>  
        // / Update the Entity  
        // / </summary>  
        // / <param name="entityToUpdate"></param>  
        public virtual void UpdateEntity(TEntity entityToUpdate)
        {
            Entities.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual IEnumerable<TEntity> GetProcedureEntity(string procedureName, params object[] parameters)
        {
            IEnumerable<TEntity> entities = Entities.FromSqlRaw(procedureName, parameters);
            return entities;
        }


    }
}