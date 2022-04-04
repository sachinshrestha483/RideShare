using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IRideIntermediatePositionRepository : IRepository<RideIntermediatePosition>
    {
        public bool Update(RideIntermediatePosition rideIntermediatePosition);

    }
}
