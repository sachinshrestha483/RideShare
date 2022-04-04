using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
 public  interface   IVehicleTypeRepository:IRepository<VehicleType>
    {
        public void Update(VehicleType vehicleType);
    }
}
