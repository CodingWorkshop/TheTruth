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
        public static ConcurrentDictionary<string, Reservation> ReservationList;
        public ReservationRepository()
        {
            ReservationList = new ConcurrentDictionary<string, Reservation>();
        }
        public Reservation GetClientAll(string clientId)
        {
            return ReservationList.TryGetValue(clientId, out var reservations) ? reservations : null;
        }

        public List<ReservationTime> GetClientReservations(string clientId)
        {
            ReservationList.TryGetValue(clientId, out var reservations);
            return reservations.Reservations;
        }

        public bool RemoveClientReservation(string clientId, long tick)
        {
            var reservation = GetClientReservations(clientId).FirstOrDefault(r => r.Tick == tick);
            if(reservation != null)
                GetClientReservations(clientId).Remove(reservation);
            return true;
        }
        public bool UpdateClientReservation(string clientId, long tick, DateTime startTime, DateTime endTime, List<string> codes)
        {
            if(HasReservationTime(GetClientReservations(clientId).Where(r => r.Tick != tick), startTime, endTime))
            {
                var reservation = GetClientReservations(clientId).Where(r => r.Tick == tick).First();
                reservation.StartTime = startTime;
                reservation.EndTime = endTime;
                reservation.Codes = codes.Any() ? codes : reservation.Codes;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddClientReservation(string clientId, DateTime startTime, DateTime endTime,
            IEnumerable<string> codes, long? tick = null)
        {
            var allReservation = GetClientReservations(clientId);
            if(HasReservationTime(allReservation, startTime, endTime))
            {
                allReservation.Add(new ReservationTime
                {
                    Tick = tick ?? DateTime.Now.Ticks,
                        StartTime = startTime,
                        EndTime = endTime,
                        Codes = codes,
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