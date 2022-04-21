using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.Utilities.RequestObject
{
    public class PaymentObject
    {
        public string PaymentId { get; set; }
        public int RideShareOfferId { get; set; }
        public double Price { get; set; }

    }
}
