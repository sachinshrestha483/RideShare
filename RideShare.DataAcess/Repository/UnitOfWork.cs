using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Utilities.Helpers.EmailHelper;
using RideShare.Utilities.Helpers.FirebaseHelper;
using RideShare.Utilities.Helpers.MessageHelper;
using RideShare.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;





        public UnitOfWork(ApplicationDbContext db,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IOptions<AppSettings> appSettings,
             IEmailSender emailSender,
             IOptions<TwilioSettings> twilioOptions,
             IOptions<FirebaseSettings> firebaseOptions

            )
        {
            _db = db;

            UserRepository = new UserRepository(
                _db,
                dataProtectionProvider,
                dataProtectionPurposeStrings,
            emailSender,
            twilioOptions
                );

            JwtTokenRepository = new JwtTokenRepository(appSettings);

            RefreshTokenRepository = new RefreshTokenRepository(_db);

            CategoryRepository = new CategoryRepository(_db);

            FirebaseRepository = new FirebaseRepository(_db, firebaseOptions);


            TravelPrefrenceRepository = new TravelPrefrenceRepository(_db);
            SubTravelPrefrenceRepository = new SubTravelPrefrenceRepository(_db);
            UserTravelPrefrenceRepository = new UserTravelPrefrenceRepository(_db);
            VehicleRepository = new VehicleRepository(_db);
            VehicleTypeRepository = new VehicleTypeRepository(_db);
            RideRepository = new RideRepository(_db);
            RideIntermediatePositionRepository = new RideIntermediatePositionRepository(_db);
            RideShareOfferRepository = new RideShareOfferRepository(_db);

        }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public IJwtTokenRepository JwtTokenRepository { get; private set; }


        public IFirebaseRepository FirebaseRepository { get; private set; }

        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }
        
        public ITravelPrefrenceRepository TravelPrefrenceRepository { get; private set; }


        public IUserTravelPrefrenceRepository UserTravelPrefrenceRepository { get; private set; }


        public ISubTravelPrefrenceRepository SubTravelPrefrenceRepository { get; private set; }


        public IVehicleRepository VehicleRepository { get; private set; }

        public IVehicleTypeRepository VehicleTypeRepository { get; private set; }

        public IRideRepository RideRepository { get; private set; }

        public IRideIntermediatePositionRepository RideIntermediatePositionRepository { get; private set; }

        public IRideShareOfferRepository RideShareOfferRepository { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
