using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
    public class RefreshTokenRequestObject
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
