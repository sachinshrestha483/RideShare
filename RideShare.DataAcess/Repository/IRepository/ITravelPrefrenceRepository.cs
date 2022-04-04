using RideShare.DataAcess.Data;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface ITravelPrefrenceRepository:IRepository<TravelPrefrence>
    {


        public bool Update(TravelPrefrence travelPrefrence);


       











    }
}
