using Microsoft.EntityFrameworkCore;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using RideShare.Models.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{

   public class RideShareOfferRepository : Repository<RideShareOffer>,IRideShareOfferRepository
    {
        private readonly ApplicationDbContext _db;
        public RideShareOfferRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public bool IsRequestForRideAlreadyCreated(int rideId, int requestcreaterUserId)
        {
            return _db.RideShareOffers.FirstOrDefault(rs=>rs.RideId==rideId && rs.UserId==requestcreaterUserId)!=null;
        }

        public RideShareOffer GetRideShareOfferForRideofUser(int userId, int rideId )
        {
            var savedRideSahreOffer = _db.RideShareOffers.FirstOrDefault(r => r.RideId == rideId && r.UserId == userId);
            return savedRideSahreOffer;
        }

        public bool UpdateRideShareOfferStatus(int id, RideShareOfferStatus status)
        {
            var savedRideShareOffer = _db.RideShareOffers.FirstOrDefault(rs=>rs.Id==id);
            savedRideShareOffer.RideShareOfferStatus = status;
            _db.SaveChanges();
            return true;
        }


        public List<RideShareOffer> GetAll(int userId,bool isApproved=false)
        {
            var rideShareRequests = _db.RideShareOffers.Include(rs => rs.Ride.Vehicle).Include(rs=>rs.Ride).ThenInclude(r=>r.IntermediatePositions).Where(rs => rs.UserId == userId);

            if(isApproved)
            {
                rideShareRequests.Where(rs => rs.RideShareOfferStatus == RideShareOfferStatus.Approved);
            }

            return rideShareRequests.ToList();
        }

        public List<RideShareOffer> GetAllOffers(int userId)
        {
            var rideShareOffers = _db.RideShareOffers.Include(rs => rs.Ride.Vehicle).Include(rs => rs.Ride).ThenInclude(r => r.IntermediatePositions).Where(rs => rs.Ride.UserId==userId);
            return rideShareOffers.ToList();
        }


        public void SetRideShareOfferForReview(int id)
        {
            var rideShareOffer = _db.RideShareOffers.FirstOrDefault(r => r.Id == id);
            if(rideShareOffer.RideShareOfferStatus== RideShareOfferStatus.Pending)
            {
                rideShareOffer.RideShareOfferStatus = RideShareOfferStatus.Viewed;
                _db.SaveChanges();
            }
        }


        public RideShareOffer GetById(int id)
        {
            var rideShareOffer = _db.RideShareOffers.Include(rs => rs.Ride.Vehicle).Include(rs => rs.Ride).ThenInclude(r => r.IntermediatePositions).FirstOrDefault(rs => rs.Id ==id);
            return rideShareOffer;
        }

    


        public bool Update(RideShareOffer rideShareOffer)
        {
            var savedRideShareOffer = _db.RideShareOffers.FirstOrDefault(rs=>rs.Id==rideShareOffer.Id);
            savedRideShareOffer.StartLocationName = rideShareOffer.StartLocationName;
            savedRideShareOffer.EndLocationName = rideShareOffer.EndLocationName;
            savedRideShareOffer.StartLocationLatitude = rideShareOffer.StartLocationLatitude;
            savedRideShareOffer.StartLocationLongitude = rideShareOffer.StartLocationLongitude;
            savedRideShareOffer.EndLocationLatitude = rideShareOffer.EndLocationLatitude;
            savedRideShareOffer.EndLocationLongitude = rideShareOffer.EndLocationLongitude;
            savedRideShareOffer.OfferedPrice = rideShareOffer.OfferedPrice;
            savedRideShareOffer.NumberOfPassengers = rideShareOffer.NumberOfPassengers;
            savedRideShareOffer.NotesForRideCreater = rideShareOffer.NotesForRideCreater;
            savedRideShareOffer.LastUpdated = DateTimeOffset.UtcNow;
            savedRideShareOffer.NotesForOfferCreator = rideShareOffer.NotesForOfferCreator;

            _db.SaveChanges();
            return true;
        }
    }
}
