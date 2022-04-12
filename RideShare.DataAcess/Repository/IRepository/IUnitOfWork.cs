using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {

        public ICategoryRepository CategoryRepository { get; }

        IUserRepository UserRepository { get; }

        IJwtTokenRepository JwtTokenRepository { get; }

        IRefreshTokenRepository RefreshTokenRepository { get; }

        IFirebaseRepository FirebaseRepository { get; }

        ITravelPrefrenceRepository TravelPrefrenceRepository { get; }

        ISubTravelPrefrenceRepository SubTravelPrefrenceRepository { get; }

        IUserTravelPrefrenceRepository UserTravelPrefrenceRepository { get; }

        IVehicleRepository VehicleRepository { get; }

        IVehicleTypeRepository VehicleTypeRepository { get; }

        IRideRepository RideRepository { get; }

        IRideIntermediatePositionRepository RideIntermediatePositionRepository { get; }

        IRideShareOfferRepository RideShareOfferRepository{ get; }


        void Save();


    }
}
