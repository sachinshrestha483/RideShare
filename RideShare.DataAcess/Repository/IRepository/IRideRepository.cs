using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IRideRepository : IRepository<Ride>
    {
        public void Update(Ride ride);
        public void UpdateNumberOfPassengers(int id, int numberofPassenger);

        public void ToggleIsAcceptingRequest(int id);

        public bool IsAcceptingRequestStatus(int id);

        public IEnumerable<Ride> LoadAllRide(int userId);



    }




}
