using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PackageDelivery.SharedKernel.Data
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> FindByInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity FindByKeyInclude(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        void Delete(int id);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity FindByKey(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
    }
}