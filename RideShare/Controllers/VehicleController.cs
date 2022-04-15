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
using RideShare.Utilities.Helpers.FirebaseHelper;
using RideShare.Utilities.RequestObject;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitofWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public VehicleController(IUnitOfWork unitOfWork,IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitofWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }



        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromForm] AddVehicleRequestObject requestObject)
        {





            var vehicle = new Vehicle();

            vehicle.Color = requestObject.Color;
            vehicle.CompanyName = requestObject.CompanyName;
            vehicle.DateOfRegistration = requestObject.DateOfRegistration;
            vehicle.LicensePlateNumber = requestObject.LicensePlateNumber;
            vehicle.ModelName = requestObject.ModelName;
            vehicle.VehicleTypeId = requestObject.VehicleTypeId;

            vehicle.PhotoPath = null;
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

            // upload photo 

            if (requestObject.VehiclePhoto != null)
            {
 
            var photoName = Guid.NewGuid().ToString() + "_" + requestObject.VehiclePhoto.FileName;

          var operation=     await _unitofWork
                    .FirebaseRepository
                    .Upload(requestObject.VehiclePhoto.OpenReadStream(),

                    photoName, FilePaths.VehiclePhoto



                    );

                if (operation == null)
                {
                    return BadRequest(new { message = "Photo Not Uploaded" });
                }


                vehicle.PhotoPath = photoName;
            }



            vehicle.UserId = user.Id;
           
            _unitofWork.VehicleRepository.Add(vehicle);
            _unitofWork.Save();

            return Ok();
        }



        [HttpPost("UpdateVehicle")]
        public async  Task<IActionResult> UpdateVehicle([FromForm] UpdateVehicleRequestObject requestObject)

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





            var vehicleFromDb = _unitofWork.VehicleRepository.Get(requestObject.Id);


            if (vehicleFromDb.UserId != user.Id)
            {
                return BadRequest();
            }

            if (vehicleFromDb == null)
            {
                return BadRequest();
            }


            var vehicle = new Vehicle();

            vehicle.Id = requestObject.Id;
            vehicle.LicensePlateNumber = requestObject.LicensePlateNumber;
            vehicle.ModelName = requestObject.ModelName;
            vehicle.VehicleTypeId = requestObject.VehicleTypeId;
            vehicle.Color = requestObject.Color;
            vehicle.CompanyName = requestObject.CompanyName;
            vehicle.DateOfRegistration = requestObject.DateOfRegistration;
            vehicle.PhotoPath = vehicleFromDb.PhotoPath;

            if (requestObject.VehiclePhoto != null)
            {

               
                var photoName = Guid.NewGuid().ToString() + "_" + requestObject.VehiclePhoto.FileName;

                var operation = await _unitofWork
                          .FirebaseRepository
                          .Upload(requestObject.VehiclePhoto.OpenReadStream(),

                          photoName, FilePaths.VehiclePhoto



                          );

                if (operation == null)
                {
                    return BadRequest(new { message = "Photo Not Uploaded" });
                }


                vehicle.PhotoPath = photoName;

// check if Photo Already there if there than delete it 
                if (vehicleFromDb.PhotoPath != null)
                {
                await    _unitofWork.FirebaseRepository.Delete(vehicleFromDb.PhotoPath, FilePaths.VehiclePhoto);
                }



            }

            _unitofWork.VehicleRepository.Update(vehicle);

            return Ok();
        }



        [HttpGet("GetUserVehicles")]
        public async  Task<IActionResult> GetUserVehicles()
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


            var vehicles = _unitofWork.VehicleRepository.GetAll(v=>v.UserId==user.Id,null, "VehicleType");



            var vehiclesDto = new List<VehicleDto>();


            foreach (var item in vehicles)
            {
                if (item.PhotoPath != null)
                {
                  var photoUrl = await _unitofWork.FirebaseRepository.GetLink(item.PhotoPath, FilePaths.VehiclePhoto);
                     item.VehiclePhotoUrl = photoUrl;
                }

                var vehicleDto = _mapper.Map<VehicleDto>(item);
                vehicleDto.DateOfRegistration = vehicleDto.DateOfRegistration;
                vehiclesDto.Add(vehicleDto);
            }

            return Ok(vehiclesDto);
        }

        
        
        
        
        
        [HttpGet("GetUserVehiclesById")]
        public async Task<IActionResult> GetUserVehicleById(int id)
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

            var vehicle = _unitofWork.VehicleRepository.Get(id);
            var vehicleType = _unitofWork.VehicleTypeRepository.Get(vehicle.VehicleTypeId);

            vehicle.VehicleType = vehicleType;

            //if (vehicle.UserId != user.Id)
            //{
            //    return BadRequest();
            //}

            if (vehicle.PhotoPath != null)
            {
             vehicle.VehiclePhotoUrl  = await  _unitofWork.FirebaseRepository.GetLink(vehicle.PhotoPath, FilePaths.VehiclePhoto);
            }

            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);





           
            return Ok(vehicleDto);
        }



    }
}
