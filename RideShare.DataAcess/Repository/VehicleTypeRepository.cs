using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{
  public  class VehicleTypeRepository:Repository<VehicleType>,IVehicleTypeRepository
    {

        private readonly ApplicationDbContext _db;
        public VehicleTypeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(VehicleType updatedVehicleType)
        {
            var vehicleType = _db.VehicleTypes.FirstOrDefault(v => v.Id == updatedVehicleType.Id);
            if (vehicleType == null)
            {
                return;
            }

            vehicleType.Name = updatedVehicleType.Name;
            vehicleType.Show = updatedVehicleType.Show;

            _db.SaveChanges();

            return;

        }
    }
}
