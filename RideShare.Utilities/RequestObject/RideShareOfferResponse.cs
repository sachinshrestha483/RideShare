using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
    public class RideShareOfferResponse
    {
        public string MessageForOfferCreator { get; set; }
        public int RideShareOfferId { get; set; }
        public int statusId { get; set; }

    }
}
