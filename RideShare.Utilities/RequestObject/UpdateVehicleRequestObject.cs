using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
   public class UpdateVehicleRequestObject
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string ModelName { get; set; }

        public string LicensePlateNumber { get; set; }
        public DateTime DateOfRegistration { get; set; }


        public string Color { get; set; }


        public int VehicleTypeId { get; set; }

        public IFormFile VehiclePhoto { get; set; }

       

    }
}
