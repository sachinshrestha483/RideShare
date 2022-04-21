using RideShare.Models.Models;
using RideShare.Models.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IRideShareOfferRepository :IRepository<RideShareOffer>
    {
        public bool Update(RideShareOffer rideShareOffer);

        public bool IsRequestForRideAlreadyCreated(int rideId, int requestcreaterUserId);

        public RideShareOffer GetRideShareOfferForRideofUser(int userId, int rideId);

        public List<RideShareOffer> GetAll(int userId, bool isApproved = false);

        public bool UpdateRideShareOfferStatus(int id, RideShareOfferStatus status);

        public List<RideShareOffer> GetAllOffers(int userId);

        public void SetRideShareOfferForReview(int id);

        public RideShareOffer GetById(int id);


        public List<int> GetAllApprovedRideOffers(int rideId);

        public List<int> GetAllDisApprovedRideOffers(int rideId);

        public (bool isSucessfull, string message) MakePayment(int rideShareOfferId, double amount, string token );
        public (bool isSucessfull, string message) IsPaymentDoable(int rideShareOfferId);




    }
}
