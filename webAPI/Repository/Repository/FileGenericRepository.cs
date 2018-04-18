using DataAccess;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Repository.Repository
{
    public class FileGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private string _filePath;

        private Dictionary<TEntity, TEntity> _entities { get; set; }

        private List<TEntity> _deletEntities { get; set; }

        public void Init(string filePath)
        {
            _filePath = Path.Combine(filePath, $"{typeof(TEntity).Name}.txt");

            if (!File.Exists(_filePath))
                File.CreateText(_filePath).Close();

            var content = File.ReadAllText(_filePath, Encoding.UTF8);

            _entities = string.IsNullOrWhiteSpace(content)
                ? new Dictionary<TEntity, TEntity>()
                : JsonConvert.DeserializeObject<List<TEntity>>(content)
                    .ToDictionary(k => k, v => default(TEntity));

            _deletEntities = new List<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.Keys;
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> where)
        {
            return _entities.Where(e => where(e.Key)).Select(e => e.Key);
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity, entity);
        }

        public void Update(TEntity entity, Func<TEntity, TEntity> updateFunc)
        {
            var target = _entities.FirstOrDefault(e => e.Key == entity).Key;

            if (target == null)
                return;

            updateFunc(target);
            _entities[entity] = target;
        }

        public void Delete(TEntity entity)
        {
            var target = _entities.FirstOrDefault(e => e.Key == entity).Key;
            _entities.Remove(target);
            _deletEntities.Add(target);
        }

        public void SaveChanges()
        {
            if(!HasChanges())
                return;
            var content = _entities.Select(e => e.Key);
            var contentString = JsonConvert.SerializeObject(content);
            File.WriteAllText(_filePath, contentString);
            _entities = _entities.ToDictionary(k => k.Key, v => default(TEntity));
            _deletEntities.Clear();
        }

        private bool HasChanges()
        {
            return _entities.Any(e => e.Value != null) || _deletEntities.Any();
        }
    }

    public class CategoryRepository : FileGenericRepository<Category>
    {
    }

    public class ClientIdentityRepository : FileGenericRepository<ClientIdentity>
    {
    }
}