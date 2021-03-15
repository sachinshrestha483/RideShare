using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.Helpers.EmailHelper
{
  public  class EmailOptions
    {
        public string SendGridKey { get; set; }//Same name as the apsetting.json
        public string SendGridUser { get; set; }//Same name as the apsetting.json

    }
}
