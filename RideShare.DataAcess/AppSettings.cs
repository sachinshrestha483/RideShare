using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess
{
  public  class AppSettings
    {
        public string JwtTokenSecret { get; set; }
        public TimeSpan TokenLifeTime { get; set; }

    }
}
