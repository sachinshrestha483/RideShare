using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RideShare.DataAcess.Repository
{
  public  class RefreshTokenRepository:IRefreshTokenRepository
    {


        private readonly ApplicationDbContext _db;

        public RefreshTokenRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _db.RefreshTokens.Add(refreshToken);

        }


        public RefreshToken GenerateRefreshToken(int userId, DateTime creationDate, DateTime expiryDate, string jwtToken, bool used)
        {

            Guid g = Guid.NewGuid();
            var refreshToken = g.ToString();

            RefreshToken refreshTokenObj = new RefreshToken()
            {
                UserId = userId,
                CreationDate = creationDate,
                ExpiryDate = expiryDate,
                JwTId = jwtToken,

                Token = refreshToken,
                Used = used


            };


            return refreshTokenObj;
        }


        public bool ValidaterefreshToken(string refreshToken, string token, User user)
        {

            var refreshTokenObj = _db.RefreshTokens.FirstOrDefault(r => (r.Token == refreshToken));


            if (user == null)
            {
                return false;
            }

            // check Whether That User has sended the Token With the refresh token..

            if (refreshTokenObj == null)
            {
                return false;
            }


            if (user.Id != refreshTokenObj.UserId)
            {
                // We require it as key by different user and the refresh token by the different user..
                return false;
            }


            if (refreshTokenObj.Used == true)
            {
                return false;
            }


            //var userId = int.Parse(JwtToken.GerUserIdByToken(token));
            //if (refreshTokenObj.UserId != userId)
            //{
            //    return false;
            //}

            if (refreshTokenObj == null)
            {
                return false;
            }


            if (refreshTokenObj.JwTId != token || refreshTokenObj.ExpiryDate >= DateTime.UtcNow)
            {


                //   return false;
                // have To make It false as make it true for the development purpose...

                //return true;
            }


            //  refreshTokenObj.Used = true;


            return true;


        }


        public void UpdateUseRefreshToken(string refreshToken)
        {

            var refreshTokenObj = _db.RefreshTokens.FirstOrDefault(r => r.Token == refreshToken);
            refreshTokenObj.Used = true;// update Or Delete The token
            _db.RefreshTokens.Update(refreshTokenObj);


        }



       public  bool Logout(string refreshToken)
        {
            var refreshTokenObject = _db.RefreshTokens.FirstOrDefault(r => r.Token == refreshToken);

            if (refreshTokenObject == null)
            {
                return false;
            }

          _db.RefreshTokens.Remove(refreshTokenObject);
            var res = _db.SaveChanges();

            if (res == 0)
            {
                return false;
            }

            return true;


        }



    }
}
