using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models.Dtos;
using RideShare.Utilities.Enums;
using RideShare.Utilities.Helpers;
using RideShare.Utilities.Helpers.EmailHelper;
using RideShare.Utilities.Helpers.FirebaseHelper;
using RideShare.Utilities.Helpers.MessageHelper;
using RideShare.Utilities.RequestObject;
using RideShare.Utilities.ResponseObject;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private TwilioSettings _twilioOptions { get; set; }


        public UserController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOptions<TwilioSettings> twilioOptions, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _twilioOptions = twilioOptions.Value;
            _mapper = mapper;


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



        [HttpPost("UploadUserPhoto")]
        public async  Task<IActionResult> AddUserPhoto([FromForm] UserProfilePhoto userProfilePhotoObject)
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


            var user = _unitOfWork.UserRepository.GetUser(int.Parse(userId));



            if (user.UserProfilePhotoPath != null)
            {
                await _unitOfWork.FirebaseRepository.Delete(user.UserProfilePhotoPath,FilePaths.UserPhoto);

            }


            var userPhotoResponseObject = new UserPhotoResponse();

            var photoName = Guid.NewGuid().ToString() + "_" + userProfilePhotoObject.userPhoto.FileName;



        var operation=  await  _unitOfWork.FirebaseRepository.Upload(userProfilePhotoObject.userPhoto.OpenReadStream(), photoName,FilePaths.UserPhoto);


            if (operation == null)
            {
                return BadRequest(new {message="Photo Not Uploaded" });
            }




            userPhotoResponseObject.link = operation;




            _unitOfWork.UserRepository.SetUserProfilePhoto(photoName, user.Id);
            
            
            return Ok(new { link=userPhotoResponseObject.link});

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
            var isUniqueEmail = _unitOfWork.UserRepository.isUniqueEmail(registerUserRequestObject.Email);
            if (!isUniqueEmail)
            {
                return BadRequest(new { message = "Email Already Used By Other Account" });
            }
            var isUniquePhone = _unitOfWork.UserRepository.isUniquePhone(registerUserRequestObject.Phone);

            if (!isUniquePhone)
            {
                return BadRequest(new { message = "Phone Already Used by Other Account" });
            }

            _unitOfWork.UserRepository.Register(registerUserRequestObject);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPost("Authenticate")]

        public async  Task<IActionResult> Authenticate(AuthenticateRequestObject authenticateRequestObject)
        {
            var user = _unitOfWork.UserRepository.Authenticate(authenticateRequestObject.email, authenticateRequestObject.Password);
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
            var userDto = _mapper.Map<UserDto>(user);
            userDto.ProfilePhotoUrl = null;

            if (user.UserProfilePhotoPath != null)
            {
                userDto.ProfilePhotoUrl = await _unitOfWork.FirebaseRepository.GetLink(user.UserProfilePhotoPath, FilePaths.UserPhoto);

            }



            return Ok(userDto);
        }


        [HttpPost("RefreshToken")]

        public async  Task<IActionResult> RefreshToken( RefreshTokenRequestObject refreshTokenRequestObject)
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
           
            var userDto = _mapper.Map<UserDto>(user);


            userDto.ProfilePhotoUrl = null;

            if (user.UserProfilePhotoPath!= null)
            {
                userDto.ProfilePhotoUrl = await _unitOfWork.FirebaseRepository.GetLink(user.UserProfilePhotoPath, FilePaths.UserPhoto);

            }
            



            return Ok(userDto);


        }



        [HttpGet("EmailVerified")]
        [Authorize]
        public IActionResult EmailVerified()
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



            var isEmailVerified = _unitOfWork.UserRepository.isEmailVerified(int.Parse(userId));


            if (isEmailVerified)
            {
                return Ok(new { EmialVerified = true });
            }

            return Ok(new { EmialVerified = false}); ;


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


        [HttpGet("PhoneVerified")]
        [Authorize]

        public IActionResult PhoneVerified()
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
                return Ok(new { PhoneVerified = true });
            }


            return Ok(new { EmailVerified = false });
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



        [HttpGet("UserPublicProfile")]
        public async Task<IActionResult> UserPublicProfile(int id)
        {
            var user = _unitOfWork.UserRepository.GetUser(id);
            if (user == null)
            {
                return BadRequest();
            }


            if (user.UserProfilePhotoPath != null)
            {
                var url = await _unitOfWork.FirebaseRepository.GetLink(user.UserProfilePhotoPath, FilePaths.UserPhoto);
                user.ProfilePhotoUrl = url;
            
            }







            var userPublicProfile = _mapper.Map<UserPublicProfileDto>(user);


            var userTravelPrefrences = _unitOfWork.UserTravelPrefrenceRepository.GetAll(t => t.UserId == id,null, "SubTravelPrefrence");

            foreach (var item in userTravelPrefrences)
            {

                userPublicProfile.UserTravelPrefrences.Add(item.SubTravelPrefrence.Name);
               
            }


            return Ok(userPublicProfile);
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
