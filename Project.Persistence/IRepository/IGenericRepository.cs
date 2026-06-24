//  <copyright file="IRepository.cs" company="PlaceholderCompany">
//  Copyright (c) PlaceholderCompany. All rights reserved.
//  </copyright>

namespace Project.Persistence.IRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");

        // / <summary>  
        // / Gets the specified identifier.  
        // / </summary>  
        // / <param name="id">The identifier.</param>  
        // / <returns></returns>  
        TEntity GetById(int id);
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        /// <summary>  
        /// Gets all.  
        /// </summary>  
        /// <returns></returns>  
        IEnumerable<TEntity> GetAll();

        // / <summary>  
        // / Finds the specified predicate.  
        // / </summary>  
        // / <param name="predicate">The predicate.</param>  
        // / <returns></returns>  
        IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);

        // / <summary>  
        // / Singles the or default.  
        // / </summary>  
        // / <param name="predicate">The predicate.</param>  
        // / <returns></returns>  
        TEntity SingleOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);

        // / <summary>  
        // / First the or default.  
        // / </summary>  
        // / <returns></returns>  
        TEntity FirstOrDefault();

        // / <summary>  
        // / Adds the specified entity.  
        // / </summary>  
        // / <param name="entity">The entity.</param>  
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);

        // / <summary>  
        // / Adds the range.  
        // / </summary>  
        // / <param name="entities">The entities.</param>  
        void AddRange(IEnumerable<TEntity> entities);

        // / <summary>  
        // / Removes the specified entity.  
        // / </summary>  
        // / <param name="entity">The entity.</param>  
        void Remove(TEntity entity);

        // / <summary>  
        // / Removes the range.  
        // / </summary>  
        // / <param name="entities">The entities.</param>  
        void RemoveRange(IEnumerable<TEntity> entities);

        // / <summary>  
        // / Removes the Entity  
        // / </summary>  
        // / <param name="entityToDelete"></param>  
        void RemoveEntity(TEntity entityToDelete);

        // / <summary>  
        // / Update the Entity  
        // / </summary>  
        // / <param name="entityToUpdate"></param>  
        void UpdateEntity(TEntity entityToUpdate);

        IEnumerable<TEntity> GetProcedureEntity(string procedureName, params object[] parameters);

        void UpdateRange(IEnumerable<TEntity> entities);
    }
}
