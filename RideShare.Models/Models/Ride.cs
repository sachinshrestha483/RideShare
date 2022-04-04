using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RideShare.Models.Models
{
    public class Ride
    {
        public int Id { get; set; }
        public string StartLocationName { get; set; }
        public string EndLocationName { get; set; }
        public string StartLocationLatitude { get; set; }
        public string StartLocationLongitude { get; set; }
        public string EndLocationLatitude { get; set; }
        public string EndLocationLongitude { get; set; }
        public DateTimeOffset  DateTimeOfRide { get; set; }
        public string Path { get; set; }
        public string RouteVia { get; set; }
        public  float DistanceinMeter { get; set; }
        public string  Note { get; set; }
        public int NumberofPassenger { get; set; }
        public double Price { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
        public int  UserId { get; set; }
        public   List<RideIntermediatePosition> IntermediatePositions{ get; set; }
    }
}
