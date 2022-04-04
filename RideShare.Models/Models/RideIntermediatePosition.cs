using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RideShare.Models.Models
{
   public class RideIntermediatePosition
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal PositionLatitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal PositionLongitude { get; set; }
        [JsonIgnore]
        public Ride Ride { get; set; }
        public int RideId { get; set; }
    }
}
