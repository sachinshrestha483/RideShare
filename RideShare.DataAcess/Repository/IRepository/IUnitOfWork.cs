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
        void Save();


    }
}
