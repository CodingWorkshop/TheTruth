using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class Reservation
    {
        public int ClientId { get; set; }
        
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
    }

}