using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
   public class VehicleDto
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


        public int UserId { get; set; }

        public string VehiclePhotoUrl { get; set; }

    }
}
