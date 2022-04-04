using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{
 public   class VehicleRepository:Repository<Vehicle>,IVehicleRepository
    {
        private readonly ApplicationDbContext _db;
        public VehicleRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public void Update(Vehicle updatedVehicle)
        {

            var vehicle = _db.Vehicle.FirstOrDefault(v => v.Id == updatedVehicle.Id);

            if (vehicle == null)
            {
                return;
            }

            vehicle.LicensePlateNumber = updatedVehicle.LicensePlateNumber;
            vehicle.CompanyName = updatedVehicle.CompanyName;
            vehicle.ModelName = updatedVehicle.ModelName;

            vehicle.PhotoPath = updatedVehicle.PhotoPath;
            vehicle.VehicleTypeId = updatedVehicle.VehicleTypeId;
            vehicle.Color = updatedVehicle.Color;
            vehicle.DateOfRegistration = updatedVehicle.DateOfRegistration;


            _db.SaveChanges();



            return;





        }




    }
}
