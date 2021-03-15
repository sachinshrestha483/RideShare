using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RideShare.DataAcess.Repository
{
   public class JwtTokenRepository:IJwtTokenRepository
	{


		private readonly AppSettings _appSettings;

		public JwtTokenRepository(IOptions<AppSettings> appSettings)
        {
			_appSettings = appSettings.Value;
		}

		public string GetJwtToken(UserClaims userClaims)
		{


			var tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.ASCII.GetBytes(_appSettings.JwtTokenSecret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{

				Subject = new ClaimsIdentity(new Claim[] {

					new Claim(ClaimTypes.Name, userClaims.UserId),
					new Claim(ClaimTypes.Role, userClaims.Role)

				}),

				Expires = DateTime.UtcNow.Add(_appSettings.TokenLifeTime),
				IssuedAt = DateTime.UtcNow,

				SigningCredentials = new SigningCredentials
				(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)


			};

			var token = tokenHandler.CreateToken(tokenDescriptor);


			return tokenHandler.WriteToken(token);
		}




		public bool VerifyJwtToken(string token)
        {
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtTokenSecret));



			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{

					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = mySecurityKey,
					ValidateLifetime=false,

				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;


			}
			return true;
		}


		public bool ValidateJWTToken(string token)
		{

			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtTokenSecret));



			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{

					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = mySecurityKey,
ValidateLifetime=false,
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			var tokenHandler1 = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler1.ReadToken(token);


			return true;
//			if (securityToken.ValidTo >= DateTime.UtcNow)
//			{
//				return true;
//			}
//			else
//			{
//				return true;

////				return false;
//			}





		}



		public string GerUserIdByToken(string token)
		{

			var userId = this.GetClaim(token, "unique_name");

			return userId;

		}


		public string GetClaim(string token, string key)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;


			var ClaimValue = securityToken.Payload.FirstOrDefault(claim => claim.Key == key).Value;

			if (ClaimValue == null)
			{
				return null;
			}
			return ClaimValue.ToString();
		}

	}
}
