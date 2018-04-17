using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        void Add(TEntity entity);

        void Update(TEntity entity, Func<TEntity, TEntity> updateFunc);

        void Delete(TEntity entity);
        
        void SaveChanges();
    }
}