using Microsoft.EntityFrameworkCore;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{
    public class RideRepository : Repository<Ride>, IRideRepository
    {
        private readonly ApplicationDbContext _db;

        public RideRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public new Ride Get(int id)
        {
            return _db.Ride.Include(r => r.IntermediatePositions).FirstOrDefault(r => r.Id == id);
        }


        public void UpdateNumberOfPassengers(int id, int numberofPassenger)
        {
            var ride = _db.Ride.FirstOrDefault(r => r.Id == id);
            if(ride.NumberofPassenger>numberofPassenger)
            {
                ride.NumberofPassenger = ride.NumberofPassenger - numberofPassenger;
            }
            else
            {
                ride.NumberofPassenger = 0;
            }
            _db.SaveChanges();
        }


        public void Update(Ride ride)
        {
            var savedRide = _db.Ride.FirstOrDefault(r => r.Id == ride.Id);
            if(savedRide!=null)
            {
                savedRide.DateTimeOfRide = ride.DateTimeOfRide;
                savedRide.DistanceinMeter = ride.DistanceinMeter;
                savedRide.EndLocationLatitude = ride.EndLocationLatitude;
                savedRide.EndLocationLongitude = ride.EndLocationLongitude;
                savedRide.EndLocationName = ride.EndLocationName;
                savedRide.IntermediatePositions = ride.IntermediatePositions;
                savedRide.Note = ride.Note;
                savedRide.NumberofPassenger = ride.NumberofPassenger;
                savedRide.Path = ride.Path;
                savedRide.Price = ride.Price;
                savedRide.RouteVia = ride.RouteVia;
                savedRide.StartLocationLatitude = ride.StartLocationLatitude;
                savedRide.StartLocationLongitude = ride.StartLocationLongitude;
                savedRide.StartLocationName = ride.StartLocationName;
                savedRide.UserId = ride.UserId;
                savedRide.VehicleId = ride.VehicleId;
                
                _db.SaveChanges();

            }

        }
    }
}
