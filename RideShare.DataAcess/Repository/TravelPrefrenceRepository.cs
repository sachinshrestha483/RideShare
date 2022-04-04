using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{
    public class TravelPrefrenceRepository :Repository<TravelPrefrence>, ITravelPrefrenceRepository
    {
        private readonly ApplicationDbContext _db;
        public TravelPrefrenceRepository(ApplicationDbContext db):base(db)
        {
            _db = db;                
        }
        public bool Update(TravelPrefrence travelPrefrence)
        {
           var updatedTravelPrefreneObj=  _db.TravelPrefrences.FirstOrDefault(t => t.Id == travelPrefrence.Id);

            updatedTravelPrefreneObj.Name = travelPrefrence.Name;
            updatedTravelPrefreneObj.show = travelPrefrence.show;

            var res= _db.SaveChanges();

            if (res == 0)
            {
                return false;
            }

            return true ;


        }
    }
}
