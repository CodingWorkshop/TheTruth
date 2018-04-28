using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class Reservation
    {
        public string ClientId { get; set; }
        
        public List<ReservationTime> Reservations { get; set; }
        public Reservation()
        {
            Reservations = new List<ReservationTime>();
        }
    }
    public class ReservationTime
    {
        public long Tick { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IEnumerable<string> Codes { get; set; }
    }

}