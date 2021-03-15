using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
    public interface IUserRepository
    {
        public bool isUniqueUser(string name);
        public void Register(string userName, string password);

        public User Authenticate(string userName, string password);


        public bool SendVerificationEmail(int userId);

        public bool SendVerificationCodePhone(int userId);

        public bool isEmailVerified(int userId);

        public bool isPhoneVerified(int userId);
        public bool VerifyEmail(string code, int userId);


        public bool VerifyPhone(string code, int userId);



        public User GetUser(int id);

    }
}
