using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using RideShare.Utilities.Enums;
using RideShare.Utilities.Helpers;
using RideShare.Utilities.Helpers.EmailHelper;
using RideShare.Utilities.Helpers.MessageHelper;
using RideShare.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RideShare.DataAcess.Repository
{
   public  class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly IEmailSender _emailSender;

        private readonly IDataProtector protector;

        private TwilioSettings _twilioOptions { get; set; }


        public UserRepository
            (ApplicationDbContext db,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IEmailSender emailSender,
            IOptions<TwilioSettings> twilioOptions

            )
        {
            _db = db;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.PasswordSecretCode);
            _emailSender = emailSender;
            _twilioOptions = twilioOptions.Value;
        }


        public User Authenticate(string username, string password)
        {

            string encryptedPassword = protector.Protect(password);





            var user = _db.
                Users
                .FirstOrDefault(
                u => u.UserName == username
                &&
                !(u.Password == encryptedPassword));

            return user;

        }

        public void Register(string userName, string password)
        {
            string encryptedPassword = protector.Protect(password);

            var user = new User();
            user.UserName = userName;
            user.Role = (int)UserRoles.Admin;
            user.Password = encryptedPassword;


            _db.Add(user);





        }





        public bool isUniqueUser(string name)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == name);

            if (user == null)
            {
                return true;
            }

            return false;
        }



        public User GetUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }



        public bool SendVerificationEmail(int userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var emailVerification = new EmailVerification();

            emailVerification.UserId = user.Id;
            emailVerification.Code = VerificationCode.GetVerificationCode().ToString();
            emailVerification.CreatedOn = DateTime.UtcNow;
            emailVerification.ValidTill = DateTime.UtcNow.AddHours(1);




            _db.EmailVerifications.Add(emailVerification);


            var res = _db.SaveChanges();


            if (res == 1)
            {
            _emailSender.SendEmail("xoxowa2754@naymio.com", "Email Verification", emailVerification.Code);

            }

            else
            {
                return false;
            }

            // Send mail ...


            return true;



        }



        public bool SendVerificationCodePhone(int userId)
        {

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }



            PhoneVerification phoneVerification = new PhoneVerification();

            phoneVerification.UserId = user.Id;
            phoneVerification.Code = VerificationCode.GetVerificationCode().ToString();
            phoneVerification.CreatedOn = DateTime.UtcNow;
            phoneVerification.ValidTill = DateTime.UtcNow.AddHours(1);




            _db.PhoneVerifications.Add(phoneVerification);


            var res = _db.SaveChanges();


            if (res == 1)
            {

                TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);

                try
                {
                    var mesage = MessageResource.Create(
                 body: "Hi",
                 from: new Twilio.Types.PhoneNumber(_twilioOptions.PhoneNumber),
                 to: new Twilio.Types.PhoneNumber("+916261675570")


                 );
                }
                catch
                {
                    return false;
                }
             

            }

            else
            {
                return false;
            }

            // Send mail ...


            return true;

        }



        public bool VerifyEmail(string code, int userId)
        {

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            var tokenFromDb = _db.EmailVerifications.FirstOrDefault(e => e.UserId == user.Id && !(e.Code == code));


            if (tokenFromDb == null)
            {
                return false;
            }


            if (tokenFromDb.ValidTill < DateTime.UtcNow)
            {
                return false;
            }


            user.isEmailVerified = true;

            _db.EmailVerifications.Remove(tokenFromDb);

            _db.SaveChanges();


            return true;




        }



        public bool VerifyPhone(string code, int userId)
        {

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            var tokenFromDb = _db.PhoneVerifications.FirstOrDefault(e => e.UserId == user.Id && (e.Code == code));


            if (tokenFromDb == null)
            {
                return false;
            }


            if (tokenFromDb.ValidTill < DateTime.UtcNow)
            {
                return false;
            }


            user.isPhoneNumberVerified = true;

            _db.PhoneVerifications.Remove(tokenFromDb);

            _db.SaveChanges();


            return true;




        }


        public bool isPhoneVerified(int userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);


            if (user == null)
            {
                return false;
            }

            if (user.isPhoneNumberVerified)
            {
                return true;
            }

            return false;


        }

        public bool isEmailVerified(int userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);


            if (user == null)
            {
                return false;
            }

            if (user.isEmailVerified)
            {
                return true;
            }

            return false;


        }



    }
}
