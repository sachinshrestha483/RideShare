using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
  public  class UserPublicProfileDto
    {
        public UserPublicProfileDto()
        {
        UserTravelPrefrences = new List<String>(); 
    }
    public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool isEmailVerified { get; set; }


        public bool isPhoneNumberVerified { get; set; }


        ///public int Role { get; set; }
        public string UserProfilePhotoPath { get; set; }


        public string ProfilePhotoUrl { get; set; }


        public List<String> UserTravelPrefrences { get; set; }


    }
}
