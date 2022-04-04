using RideShare.Models.Models;
using RideShare.Utilities.Helpers.RequestHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IUserTravelPrefrenceRepository:IRepository<UserTravellPrefrences>
    {

        public void Update();
        public int isPrefrenceThere(IsPrefrenceThereRequest obj);
        public void UpdateUserTravelPrefrence(UpdateUserTravelPrefrence obj);

        public UserTravellPrefrences getUserTravelPrefrenceByTravelPrefrenceId(UserTravelPrefrenceByTravelPrefrenceId reqObj);

    }
}
