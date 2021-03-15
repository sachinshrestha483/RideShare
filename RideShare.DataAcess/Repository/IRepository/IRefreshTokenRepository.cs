using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Repository.IRepository
{
  public  interface IRefreshTokenRepository
    {

        public bool ValidaterefreshToken(string refreshToken, string token, User user);
        public RefreshToken GenerateRefreshToken(int userId, DateTime creationDate, DateTime expiryDate, string jwtToken, bool used);
        public void AddRefreshToken(RefreshToken refreshToken);

        public bool Logout(string refreshToken);

        public void UpdateUseRefreshToken(string refreshToken);

    }
}
