using System;
using System.Collections.Generic;

namespace FlightData.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Origin { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public DateTime DepartureTime { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
