using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Utilities.Helpers.EmailHelper;
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
             IOptions<TwilioSettings> twilioOptions
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


        }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public IJwtTokenRepository JwtTokenRepository { get; private set; }


        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }
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
