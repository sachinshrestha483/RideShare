using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
  public interface  IVehicleRepository:IRepository<Vehicle>
    {

        public void Update(Vehicle vehicle);

    }
}
