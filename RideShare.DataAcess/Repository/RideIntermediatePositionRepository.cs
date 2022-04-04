using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository
{
    class RideIntermediatePositionRepository :Repository<RideIntermediatePosition>, IRideIntermediatePositionRepository
    {
         private readonly ApplicationDbContext _db;

        public RideIntermediatePositionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool Update(RideIntermediatePosition rideIntermediatePosition)
        {
            return true;
        }
    }
}
