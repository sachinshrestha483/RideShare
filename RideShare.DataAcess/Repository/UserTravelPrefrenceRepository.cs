using Microsoft.EntityFrameworkCore;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using RideShare.Utilities.Helpers.RequestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RideShare.DataAcess.Repository
{
   public class UserTravelPrefrenceRepository : Repository<UserTravellPrefrences>, IUserTravelPrefrenceRepository
    {


        private readonly ApplicationDbContext _db;


        public UserTravelPrefrenceRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public void UpdateUserTravelPrefrence(UpdateUserTravelPrefrence obj)
        {


         var userTravelPrefrenceObj=   _db.UserTravellPrefrences.FirstOrDefault(t => t.Id == obj.UserTravelPrefrenceId);

            if (userTravelPrefrenceObj == null)
            {
                return;
            }


            userTravelPrefrenceObj.SubTravelPrefrenceId = obj.SubTravelPrefrenceId;


            _db.UserTravellPrefrences.Update(userTravelPrefrenceObj);

            _db.SaveChanges();


        }



        public UserTravellPrefrences getUserTravelPrefrenceByTravelPrefrenceId(UserTravelPrefrenceByTravelPrefrenceId reqObj)
        {

            var userTravelPrefrences = _db.UserTravellPrefrences.Where(u => u.UserId == reqObj.userId).ToList();



//            var reqTravelPrefrence = _db.UserTravellPrefrences.Include(s => s.SubTravelPrefrence).FirstOrDefault(s => s.Id == reqObj.travelPrefrenceId).SubTravelPrefrence;


            foreach (var item in userTravelPrefrences)
            {
                var travelPrefrence = _db
                    .SubTravelPrefrences
                    .Include(t => t.TravelPrefrence)
                    .FirstOrDefault(t => t.id == item.SubTravelPrefrenceId)
                    .TravelPrefrence;
                
                if (travelPrefrence.Id == reqObj.travelPrefrenceId )
                {
                    return item;
                }
            
            }


            return null;
        }

        public int isPrefrenceThere(IsPrefrenceThereRequest obj )
        {

            var travelPrefrence = _db.SubTravelPrefrences.Include(p=>p.TravelPrefrence).FirstOrDefault(p => p.id == obj.SubUserPrefrenceId).TravelPrefrence;


            var userPrefrences = _db.UserTravellPrefrences.Where(p=>p.UserId==obj.UserId);

            foreach (var item in userPrefrences)
            {
                var itemTravelPrefrence = _db.SubTravelPrefrences.Include(p => p.TravelPrefrence).FirstOrDefault(p => p.id == item.SubTravelPrefrenceId).TravelPrefrence;
                
                if (itemTravelPrefrence.Id == travelPrefrence.Id)
                {
                    return item.Id;
                }

            }

            return 0;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
