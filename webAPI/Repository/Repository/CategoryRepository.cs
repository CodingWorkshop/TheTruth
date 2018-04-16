using DataAccess;
using Repository.Interface;
using System;
using System.Collections.Generic;

namespace Repository.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        public IEnumerable<Category> Get()
        {
            throw new NotImplementedException();
        }

        public void Add(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}