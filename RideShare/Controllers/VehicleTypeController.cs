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
using RideShare.Utilities.RequestObject;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public VehicleTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("GetAllVehicleType")]
        public IActionResult GetAllVehicleType()
        {
            var vehicleTypes = _unitOfWork.VehicleTypeRepository.GetAll();

            var vehiclestypesDto = new List<VehicleTypeDto>();

            foreach (var item in vehicleTypes)
            {
                var vehicleTypeDto = _mapper.Map<VehicleTypeDto>(item);
                vehiclestypesDto.Add(vehicleTypeDto);
            }
            
            return Ok(vehiclestypesDto);

        }



        [HttpPost("AddVehicleType")]
        public IActionResult AddVehiceType(AddVehicleTypeRequestObject  reqObject)
        {
            var vehicleType = new VehicleType();
            vehicleType.Name = reqObject.Name;
            vehicleType.Show = reqObject.Show;
            _unitOfWork.VehicleTypeRepository.Add(vehicleType);
            _unitOfWork.Save();

            return Ok();
        }


        [HttpPost("UpdateVehicleType")]
        public IActionResult UpdateVehiceType(UpdateVehicleTypeRequestObject reqObject)
        {
            var vehicleType = new VehicleType();
            vehicleType.Name = reqObject.Name;
            vehicleType.Id = reqObject.id;

            vehicleType.Show = reqObject.Show;

            _unitOfWork.VehicleTypeRepository.Update(vehicleType);

            return Ok();
        }



        [HttpGet("GetVehicleTypeById")]
        public IActionResult GetVehiceTypeById(int id)
        {
            var vehicleType = _unitOfWork.VehicleTypeRepository.Get(id);

            if (vehicleType == null)
            {
                return BadRequest();
            }

            var vehicleTypeDto = _mapper.Map<VehicleTypeDto>(vehicleType);

            return Ok(vehicleTypeDto);

        }







    }
}
