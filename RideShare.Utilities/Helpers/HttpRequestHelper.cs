using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.Utilities.Helpers
{
   public class HttpRequestHelper
    {
        public static string GetJwtToken(IHttpContextAccessor httpContextAccessor)
        {
            var requestHeaderAutheticationSection = httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(c => c.Key == "Authorization").Value;
            
            string[] authenticationObject = requestHeaderAutheticationSection[0].Split(" ");
            var jwtToken = authenticationObject[authenticationObject.Length - 1];
            return jwtToken;
        }

        public static string GetRefreshToken(IHttpContextAccessor httpContextAccessor)
        {
            var refreshTokenfromHeader = httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(c => c.Key == "refreshtoken").Value;
          
            return refreshTokenfromHeader;
        }
    }
}
