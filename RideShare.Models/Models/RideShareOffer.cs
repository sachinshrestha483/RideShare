using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using RideShare.Models.Models.Enums;


namespace RideShare.Models.Models
{
   public class RideShareOffer
    {
        public int Id { get; set; }
        //[JsonIgnore]
        public Ride Ride { get; set; }
        public int RideId { get; set; }
        public int NumberOfPassengers { get; set; }
        public string StartLocationName { get; set; }
        public string EndLocationName { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal StartLocationLatitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal StartLocationLongitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal EndLocationLatitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal EndLocationLongitude { get; set; }
        public int OfferedPrice { get; set; }
        public string  NotesForRideCreater{ get; set; }
        public string NotesForOfferCreator { get; set; }
        public RideShareOfferStatus RideShareOfferStatus { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public int DistancefromInitialLocation{ get; set; }
        public int DistancefromFinalLocation { get; set; }
        public string OverlappingPath { get; set; }
        public int  UserId { get; set; }
        public bool  IsPaymentDone { get; set; }
        public string PaymentToken{ get; set; }
        public double OnlineCollectedAmount { get; set; }

    }
}
