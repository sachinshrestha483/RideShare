using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models
{
  public   class SubTravelPrefrence
    {
        public int id { get; set; }
        public string Name { get; set; }

        public TravelPrefrence TravelPrefrence { get; set; }

        public int TravelPrefrenceId { get; set; }

        public bool show { get; set; }


    }
}
