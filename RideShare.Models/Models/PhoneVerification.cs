using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models
{
  public  class PhoneVerification
    {
        public int Id { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public string Code { get; set; }


        public DateTime CreatedOn { get; set; }


        public DateTime ValidTill { get; set; }


    }
}
