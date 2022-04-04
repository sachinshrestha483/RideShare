using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
   public class RideDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public List<RidePathPosition> RidePath { get; set; }
        public List<List<decimal>> RidePath { get; set; }
        public RidePosition StartPosition { get; set; }
        public RidePosition EndPosition { get; set; }
        public List<RidePosition> IntermediatePositions { get; set; }
        public DateTimeOffset RideDateTime { get; set; }
        public double RideDistance { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
        public int NumberofPassenger { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
        public string RideVia { get; set; }

    }
}
