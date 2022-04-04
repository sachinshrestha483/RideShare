using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using RideShare.Models.Models.Dtos;
using RideShare.Utilities.Helpers;
using RideShare.Utilities.Helpers.RequestHelper;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTravelPrefrenceController : ControllerBase
    {


        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTravelPrefrenceController(IUnitOfWork unitofWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }




        [HttpGet("GetUserTravelPrefrencyByTravelPrefrenceId")]

        public IActionResult GetUserTravelPrefrencyByTravelPrefrenceId(int id)
        {
            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);
            if (jwtToken == null)
            {
                return BadRequest();
            }

            var userId = _unitofWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }


            var user = _unitofWork.UserRepository.GetUser(int.Parse(userId));

            if (user == null)
            {
                return BadRequest();
            }


            var userTravelPrefrenceByTravelPrefrenceId = new UserTravelPrefrenceByTravelPrefrenceId();

            userTravelPrefrenceByTravelPrefrenceId.userId = user.Id;
            userTravelPrefrenceByTravelPrefrenceId.travelPrefrenceId = id;



            var subTravelPrefrence =

            _unitofWork
                .UserTravelPrefrenceRepository
                .getUserTravelPrefrenceByTravelPrefrenceId(userTravelPrefrenceByTravelPrefrenceId);


            return Ok(subTravelPrefrence);

        }


        [HttpGet("GetUserTravelPrefrences")]

        public IActionResult GetUserTravelPrefrences()
        {

            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);
            if (jwtToken == null)
            {
                return BadRequest();
            }

            var userId = _unitofWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }


            var user = _unitofWork.UserRepository.GetUser(int.Parse(userId));

            if (user == null)
            {
                return BadRequest();
            }
            var travelPrefrences = _unitofWork.UserTravelPrefrenceRepository.GetAll(t => t.UserId == user.Id, null, "SubTravelPrefrence");

            var subTravelPrefrences = new List<string>();


            foreach (var item in travelPrefrences)
            {
                subTravelPrefrences.Add(item.SubTravelPrefrence.Name);
            }


            return Ok(subTravelPrefrences);
        }



        [HttpPost("AddUserTravelPrefrence")]
        public IActionResult AddUserTravelPrefrence(UserTravelPrefrencesDto userTravelPrefrencesDto)
        {



            var jwtToken = HttpRequestHelper.GetJwtToken(_httpContextAccessor);
            if (jwtToken == null)
            {
                return BadRequest();
            }

            var userId = _unitofWork.JwtTokenRepository.GerUserIdByToken(jwtToken);

            if (userId == null)
            {
                return BadRequest();

            }


            var user = _unitofWork.UserRepository.GetUser(int.Parse(userId));

            if (user == null)
            {
                return BadRequest();
            }

            // only one can be selected from each section
           
            var isPrefrenceThere =new  IsPrefrenceThereRequest();
            isPrefrenceThere.UserId = user.Id;
            isPrefrenceThere.SubUserPrefrenceId = userTravelPrefrencesDto.SubTravelPrefrenceId;
            var prefrenceId = _unitofWork.UserTravelPrefrenceRepository.isPrefrenceThere(isPrefrenceThere);


            if (prefrenceId != 0)
            {
                // update it...]
                
                var updateUserTravelPrefrenceObj= new  UpdateUserTravelPrefrence();

                updateUserTravelPrefrenceObj.UserTravelPrefrenceId = prefrenceId;
                updateUserTravelPrefrenceObj.SubTravelPrefrenceId= userTravelPrefrencesDto.SubTravelPrefrenceId;

                _unitofWork.UserTravelPrefrenceRepository.UpdateUserTravelPrefrence(updateUserTravelPrefrenceObj);

                return Ok();
            }




            var userTravelPrefrence =_mapper.Map<UserTravellPrefrences>(userTravelPrefrencesDto);

            userTravelPrefrence.UserId = user.Id;
            _unitofWork.UserTravelPrefrenceRepository.Add(userTravelPrefrence);

            _unitofWork.Save();
            
            
            return Ok();
        }


    }
}
