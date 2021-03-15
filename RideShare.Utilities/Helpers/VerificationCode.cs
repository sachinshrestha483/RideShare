using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.Helpers
{
   public class VerificationCode
    {
        public static int GetVerificationCode()
        {

            Random r = new Random();
            int randomNumber = r.Next(100000, 500000);
            return randomNumber;
        }
    }
}
