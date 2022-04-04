using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
   public class FindRideDto
    {
        public DateTimeOffset RideDateTime { get; set; }
        public RidePosition StartPosition { get; set; }
        public RidePosition EndPosition { get; set; }
        public int NumberofPassenger { get; set; }
        public int UserId { get; set; }
    }
}
