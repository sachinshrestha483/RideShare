using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RideShare.Models.Models
{
   public class Vehicle
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ModelName { get; set; }


        public string LicensePlateNumber { get; set; }
        public DateTime DateOfRegistration { get; set; }


        public string Color { get; set; }

        public VehicleType VehicleType { get; set; }

        public int VehicleTypeId { get; set; }

        public string PhotoPath { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public int UserId { get; set; }

        [NotMapped]
        public string VehiclePhotoUrl { get; set; }
    }
}
