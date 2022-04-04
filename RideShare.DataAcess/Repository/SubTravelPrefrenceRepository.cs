using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{
    public class SubTravelPrefrenceRepository : Repository<SubTravelPrefrence>, ISubTravelPrefrenceRepository
    {
        private readonly ApplicationDbContext _db;
        
        public SubTravelPrefrenceRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }



        public bool Update(SubTravelPrefrence subTravelPrefrenceObj)
        {

var updatedsubTravelPrefrenceObj = _db.SubTravelPrefrences.FirstOrDefault(s => s.id == subTravelPrefrenceObj.id);


            if (updatedsubTravelPrefrenceObj == null)
            {
                return false;
            }

            updatedsubTravelPrefrenceObj.Name = subTravelPrefrenceObj.Name;
            updatedsubTravelPrefrenceObj.TravelPrefrenceId = subTravelPrefrenceObj.TravelPrefrenceId;
            updatedsubTravelPrefrenceObj.show = subTravelPrefrenceObj.show;

            _db.SaveChanges();
            return true;

        }
    }
}
