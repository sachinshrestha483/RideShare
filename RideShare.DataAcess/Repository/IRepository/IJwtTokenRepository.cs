using RideShare.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
   public interface IJwtTokenRepository
    {
        public string GetJwtToken(UserClaims userClaims);
        public bool ValidateJWTToken(string token);

        public string GerUserIdByToken(string token);

        public bool VerifyJwtToken(string token);

        public string GetClaim(string token, string key);

    }
}
