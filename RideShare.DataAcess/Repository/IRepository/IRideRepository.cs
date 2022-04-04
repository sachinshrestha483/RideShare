using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IRideRepository : IRepository<Ride>
    {
        public void Update(Ride ride);
    }

   


}
