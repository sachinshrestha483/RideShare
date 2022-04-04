using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models
{
   public class UserTravellPrefrences
    {

        public int Id { get; set; }
        public User  User{ get; set; }

        public int UserId { get; set; }


        public SubTravelPrefrence SubTravelPrefrence { get; set; }

        public int  SubTravelPrefrenceId { get; set; }




    }
}
