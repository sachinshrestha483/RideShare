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
    public class TravelPrefrenceController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public TravelPrefrenceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        
        
        
        [HttpGet("GetTravelPrefrecneById")]

        public IActionResult GetTravelPrefrenceById(int id)
        {

            var travelPrefrence = _unitOfWork.TravelPrefrenceRepository.Get(id);

            if (travelPrefrence == null)
            {
                return BadRequest();
            }
          
            return Ok(travelPrefrence);
        }






        [HttpPost("AddTravelPrefrence")]

            public IActionResult AddTravelPrefrence(TravelPrefrenceRequest travelPrefrenceRequestObj)
            {
                var travelPrefrenceonObj = new TravelPrefrence();
                travelPrefrenceonObj.Name = travelPrefrenceRequestObj.name;

                _unitOfWork.TravelPrefrenceRepository.Add(travelPrefrenceonObj);
                _unitOfWork.Save();

                return Ok();

            }

        [HttpPost("EditTravelPrefrence")]

        public IActionResult EditTravelPrefrence(EditTravelPrefrenceRequestObject editTravelPrefrenceRequestObj)
        {

              var isUpdated=_unitOfWork.TravelPrefrenceRepository.Update(editTravelPrefrenceRequestObj.TravelPrefrence);


            if (!isUpdated)
            {
                return BadRequest();
            }

            return Ok();


        }


        [HttpGet("GetTravelPrefrences")]
        public IActionResult GetTravellPrefreces()
        {
            var travellingPrefrecnes = _unitOfWork.TravelPrefrenceRepository.GetAll();

            var travellPrefreceDtos = new List<TravelPrefrenceDto>();
            foreach (var item in travellingPrefrecnes)
            {
                var travelprefreceDtoItem = _mapper.Map<TravelPrefrenceDto>(item);
                travellPrefreceDtos.Add(travelprefreceDtoItem);
            
            }


            return Ok(travellPrefreceDtos);


        }


    }
}
