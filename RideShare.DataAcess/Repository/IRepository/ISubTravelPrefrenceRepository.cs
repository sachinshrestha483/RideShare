using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface ISubTravelPrefrenceRepository:IRepository<SubTravelPrefrence>
    {

        public bool Update(SubTravelPrefrence subTravelPrefrenceObj);

    }
}
