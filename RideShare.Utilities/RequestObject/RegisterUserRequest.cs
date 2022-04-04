using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
   public class RegisterUserRequest
    {
        [StringLength(15)]
        [Required]

        public string FirstName { get; set; }

        [StringLength(15)]
        [Required]

        public string LastName { get; set; }

        [StringLength(30, MinimumLength = 9, ErrorMessage = "Minimum Password Length 9")]

        public string Password { get; set; }
        [Email]
        [Required]

        public string  Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10
            )]

        public string Phone { get; set; }





    }
}
