using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
  public  class UserTravelPrefrencesDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }



        public int SubTravelPrefrenceId { get; set; }
    }
}
