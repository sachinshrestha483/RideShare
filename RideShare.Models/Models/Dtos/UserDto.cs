using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
  public  class UserDto
    {

        public int id { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool isEmailVerified { get; set; }


        public bool isPhoneNumberVerified { get; set; }


        public int Role { get; set; }

        public string Token { get; set; }


        public string RefreshToken { get; set; }


        public string ProfilePhotoUrl { get; set; }



    }
}
