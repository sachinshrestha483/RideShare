using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Models.Models.Dtos
{
   public class OverLappingRideDto
    {

        public Ride Ride { get; set; }
        public List<List<decimal>> OverLappingPath { get; set; }
        public int OverLappingPathStartIndex { get; set; }
        public int OverLappingPathEndIndex { get; set; }
        public int RideId { get; set; }
        public int  DistanceFromInitialPosition { get; set; }
        public int DistanceFromFinalPosition { get; set; }




    }
}
