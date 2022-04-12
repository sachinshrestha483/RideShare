using RideShare.Models.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
  public  class GetOverlappingIndexes
    {
        public RidePosition StartPosition { get; set; }
        public RidePosition EndPosition { get; set; }
        public int RideId { get; set; }
    }
}
