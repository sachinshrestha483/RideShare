using Microsoft.AspNetCore.Http;
using RideShare.Models.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
   public class AddVehicleRequestObject
   {


        public string CompanyName { get; set; }

        public string ModelName { get; set; }

        public string LicensePlateNumber { get; set; }
        public DateTime DateOfRegistration { get; set; }


        public string Color { get; set; }


        public int VehicleTypeId { get; set; }

        public IFormFile VehiclePhoto { get; set; }

    }
}
