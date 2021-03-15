using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RideShare.Models.Models
{
public class RefreshToken
    {

        [Key]
        public string Token { get; set; }

        public string JwTId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Used { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
