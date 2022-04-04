using RideShare.Models.Models;
using RideShare.Utilities.RequestObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IUserRepository
    {
        public bool isUniqueEmail(string email);

        public bool isUniquePhone(string phoneNumber);


        public void Register(RegisterUserRequest requestUser);



        public User Authenticate(string email, string password);


        public bool SendVerificationEmail(int userId);

        public bool SendVerificationCodePhone(int userId);

        public bool isEmailVerified(int userId);
        public bool SetUserProfilePhoto(string url, int userId);

        public bool isPhoneVerified(int userId);
        public bool VerifyEmail(string code, int userId);


        public bool VerifyPhone(string code, int userId);



        public User GetUser(int id);

    }
}
