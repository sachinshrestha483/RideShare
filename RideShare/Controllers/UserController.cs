using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Utilities.Enums;
using RideShare.Utilities.Helpers;
using RideShare.Utilities.Helpers.EmailHelper;
using RideShare.Utilities.Helpers.MessageHelper;
using RideShare.Utilities.RequestObject;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private TwilioSettings _twilioOptions { get; set; }


        public UserController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOptions<TwilioSettings> twilioOptions)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _twilioOptions = twilioOptions.Value;


        }




        
        [HttpGet("SendSMS")]

        public IActionResult SendSMS()
        {
            TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);
            
            
                var mesage = MessageResource.Create(
                    body: "Hi",
                    from: new Twilio.Types.PhoneNumber(_twilioOptions.PhoneNumber),
                    to: new Twilio.Types.PhoneNumber("+916261675570")


                    );
            
           

            return Ok();

        }




        [HttpPost("Logout")]


        public IActionResult Logout()
        {
            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);
            if (jwtToken == null)
            {
                return BadRequest();
            }
            var refreshToken = HttpRequestHelper.GetRefreshToken(_httpContextAccessor);

            if (refreshToken == null)
            {
                return BadRequest();
            }

            
            

            var tokenVerified = _unitOfWork.JwtTokenRepository.VerifyJwtToken(jwtToken);


            if (!tokenVerified)
            {
                return BadRequest();

            }




            var userId = _unitOfWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }
            var user = _unitOfWork.UserRepository.GetUser(int.Parse(userId));





            var isRefreshTokenValid = _unitOfWork.RefreshTokenRepository.ValidaterefreshToken(refreshToken, jwtToken, user);

            if (!isRefreshTokenValid)
            {
                return BadRequest();
            }



            // logout the user

var logout=            _unitOfWork.RefreshTokenRepository.Logout(refreshToken);



            if (logout == false)
            {
                return BadRequest();
            }


            return Ok();

        }




        [HttpGet("VerifyToken")]
        
        public IActionResult VerifyToken()
        {
            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);
            if (jwtToken == null)
            {
                return BadRequest();
            }
            var tokenVerified = _unitOfWork.JwtTokenRepository.VerifyJwtToken(jwtToken);


            if (tokenVerified == false)
            {
                return BadRequest();
            }

            else
            {
                return Ok();

            }

        }



        [HttpGet("GetMessage")]
        //[Authorize(Roles ="jkjjkhjhg")]
        public IActionResult GetMessage()
        {
            return Ok(new { message="Here Is the Mesage"});
        }


        [HttpPost("SignUp")]

        public IActionResult SignUp(RegisterUserRequest registerUserRequestObject)
        {
            var isUniqueUser = _unitOfWork.UserRepository.isUniqueUser(registerUserRequestObject.Name);
            if (!isUniqueUser)
            {
                return BadRequest(new { message = "UserName Already Taken" });
            }
            _unitOfWork.UserRepository.Register(registerUserRequestObject.Name, registerUserRequestObject.Password);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPost("Authenticate")]

        public IActionResult Authenticate(AuthenticateRequestObject authenticateRequestObject)
        {
            var user = _unitOfWork.UserRepository.Authenticate(authenticateRequestObject.Name, authenticateRequestObject.Password);
            if (user == null)
            {
                return BadRequest(new { messsage = "User Name Or Password Not Valid" });
            }

            var userClaims = new UserClaims();

            userClaims.UserId = user.Id.ToString();
            userClaims.Role = UserRoles.Admin.ToString();

            var jwtToken = _unitOfWork.JwtTokenRepository.GetJwtToken(userClaims);
            var refreshToken = _unitOfWork.RefreshTokenRepository.GenerateRefreshToken(user.Id, DateTime.UtcNow, DateTime.UtcNow.AddMonths(6), jwtToken, false);
            _unitOfWork.RefreshTokenRepository.AddRefreshToken(refreshToken);
            _unitOfWork.Save();
            user.Token = jwtToken;
            user.RefreshToken = refreshToken.Token;
            return Ok(user);
        }


        [HttpPost("RefreshToken")]

        public IActionResult RefreshToken( RefreshTokenRequestObject refreshTokenRequestObject)
        {

            var isJwtTokenValid = _unitOfWork.JwtTokenRepository.ValidateJWTToken(refreshTokenRequestObject.Token);
            if (!isJwtTokenValid)
            {
                return BadRequest();
            }

            var userId = _unitOfWork.JwtTokenRepository.GerUserIdByToken(refreshTokenRequestObject.Token);

            if (userId == null)
            {
                return BadRequest();

            }
            var user = _unitOfWork.UserRepository.GetUser(int.Parse(userId));

            if (user == null)
            {
                return BadRequest();

            }


            var isRefreshTokenValid = _unitOfWork.RefreshTokenRepository.ValidaterefreshToken(refreshTokenRequestObject.RefreshToken, refreshTokenRequestObject.Token, user);



            
            if (!isRefreshTokenValid)
            {
                return BadRequest();
            }

            _unitOfWork.RefreshTokenRepository.UpdateUseRefreshToken(refreshTokenRequestObject.RefreshToken);

            _unitOfWork.Save();



            
            var userClaims = new UserClaims();
            userClaims.UserId = user.Id.ToString();
            userClaims.Role = UserRoles.Admin.ToString();

            
            var newJwtToken = _unitOfWork.JwtTokenRepository.GetJwtToken(userClaims);
            
            var newRefreshToken = _unitOfWork
                .RefreshTokenRepository
                .GenerateRefreshToken
                (user.Id, DateTime.UtcNow, DateTime.UtcNow.AddMonths(6), refreshTokenRequestObject.Token, false);
            
            
            _unitOfWork.RefreshTokenRepository.AddRefreshToken(newRefreshToken);
            _unitOfWork.Save();
           
            
            
            user.Token = newJwtToken;
            user.RefreshToken = newRefreshToken.Token;
            return Ok(user);


        }


        [HttpGet("SendVerificationEmail")]
        [Authorize]

        public IActionResult GetVerifyEmail()
        {
            //var requestHeaderAutheticationSection = _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(c=>c.Key== "Authorization").Value;
            //string[] authenticationObject = requestHeaderAutheticationSection[0].Split(" ");
            //var jwtToken = authenticationObject[authenticationObject.Length - 1];


            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);

            if (jwtToken == null)
            {
                return BadRequest();
            }
            var userId = _unitOfWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }



            var isEmailVerified = _unitOfWork.UserRepository.isEmailVerified(int.Parse(userId));


            if (isEmailVerified)
            {
                return BadRequest(new { Message = "Email Already Verified" });
            }


            var SendVerificationMail = _unitOfWork.UserRepository.SendVerificationEmail(int.Parse(userId));




            if (!SendVerificationMail)
            {
                return BadRequest();
            }











            return Ok();
        }


        [HttpGet("SendVerificationPhoneCode")]
        public IActionResult GetVerifyPhoneNumber()
        {
            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);

            if (jwtToken == null)
            {
                return BadRequest();
            }




            var userId = _unitOfWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }


            var isPhoneVerified = _unitOfWork.UserRepository.isPhoneVerified(int.Parse(userId));


            if (isPhoneVerified)
            {
                return BadRequest(new { Message = "Phone Already Verified" });
            }


            var sendPhoneCode = _unitOfWork.UserRepository.SendVerificationCodePhone(int.Parse(userId));

            if (!sendPhoneCode)
            {
                return BadRequest();
            }

            return Ok();
        }



        [HttpPost("VerifyPhone")]
        public IActionResult VerifyPhone(VerifyPhoneRequestObject   verifyPhoneRequestObject)
        {
            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);

            if (jwtToken == null)
            {
                return BadRequest();
            }
            var userId = _unitOfWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }

            var verifyCode = _unitOfWork.UserRepository.VerifyPhone(verifyPhoneRequestObject.Code, int.Parse(userId));

            if (!verifyCode)
            {
                return BadRequest();

            }

            return Ok();
        }







        [HttpPost("VerifyEmail")]
        public IActionResult VerifyEmail(VerifyEmailObjectRequest verifyEmailObjectRequest)
        {
            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);

            if (jwtToken == null)
            {
                return BadRequest();
            }
            var userId = _unitOfWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }

            var verifyCode = _unitOfWork.UserRepository.VerifyEmail(verifyEmailObjectRequest.Code, int.Parse(userId));

            if (!verifyCode)
            {
                return BadRequest();

            }

            return Ok();
        }



    }
}
