using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();
    }
}