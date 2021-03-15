using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RideShare.Models.Models
{
   public class User
    {

        public int Id { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

        public bool isEmailVerified { get; set; }


        public bool isPhoneNumberVerified { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }

        [NotMapped]
        public string Token { get; set; }


        [NotMapped]
        public string RefreshToken { get; set; }
    }
}
