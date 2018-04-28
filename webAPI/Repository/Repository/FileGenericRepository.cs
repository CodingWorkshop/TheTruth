using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataAccess;
using Newtonsoft.Json;
using Repository.Interface;

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
                _filePath = Path.Combine(filePath, $"{typeof(TEntity).Name}.json");

                if(!File.Exists(_filePath))
                    File.CreateText(_filePath).Close();

                var content = File.ReadAllText(_filePath, Encoding.UTF8);

                _entities = string.IsNullOrWhiteSpace(content) ?
                    new Dictionary<TEntity, TEntity>() :
                    JsonConvert.DeserializeObject<List<TEntity>>(content)
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

                if(target == null)
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

    public class CategoryRepository : FileGenericRepository<Category> { }

    public class ClientIdentityRepository : FileGenericRepository<ClientIdentity> { }
    public class ReservationRepository : FileGenericRepository<Reservation>
    {
        public static ConcurrentDictionary<int, Reservation> ReservationList;
        public ReservationRepository()
        {
            ReservationList = new ConcurrentDictionary<int, Reservation>();
        }
        public Reservation GetClientAll(int clientId)
        {
            return ReservationList.TryGetValue(clientId, out var reservations) ? reservations : null;
        }

        public List<ReservationTime> GetClientReservations(int clientId)
        {
            ReservationList.TryGetValue(clientId, out var reservations);
            return reservations.Reservations;
        }

        public bool RemoveClientReservation(int clientId, long tick)
        {
            var reservation = GetClientReservations(clientId).FirstOrDefault(r => r.Tick == tick);
            if(reservation != null)
                GetClientReservations(clientId).Remove(reservation);
            return true;
        }
        public bool UpdateClientReservation(int clientId, long tick, DateTime startTime, DateTime endTime)
        {
            if(HasReservationTime(GetClientReservations(clientId).Where(r => r.Tick != tick), startTime, endTime))
            {
                RemoveClientReservation(clientId, tick);
                return AddClientReservation(clientId, startTime, endTime, tick);
            }
            else
            {
                return false;
            }
        }

        public bool AddClientReservation(int clientId, DateTime startTime, DateTime endTime, long? tick = null)
        {
            var allReservation = GetClientReservations(clientId);
            if(HasReservationTime(allReservation, startTime, endTime))
            {
                allReservation.Add(new ReservationTime
                {
                    Tick = tick ?? DateTime.Now.Ticks,
                        StartTime = startTime,
                        EndTime = endTime,
                });
            }
            else
            {
                return false;
                //throw new Exception($"Has duplicated Reservation Time {startTime} to {endTime}");
            }

            return true;
        }

        private bool HasReservationTime(IEnumerable<ReservationTime> allReservation, DateTime startTime, DateTime endTime)
        {
            return allReservation.Any(r => r.StartTime > endTime && r.EndTime < startTime);
        }
    }
}