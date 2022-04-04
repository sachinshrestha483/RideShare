using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
    public class RidePosition
    {
        public string Name { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }



    }
}
