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
    public class SubTravelPrefrenceController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubTravelPrefrenceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpPost("AddSubTravelPrefrence")]
        public IActionResult AddSubTravelPrefrence(  SubTravelPrefrenceDto SubTravelPrefrenceRequestObj)
        {

            var subTravelPrefrence = _mapper.Map<SubTravelPrefrence>(SubTravelPrefrenceRequestObj);

            _unitOfWork
                .SubTravelPrefrenceRepository
                .Add(subTravelPrefrence);


            _unitOfWork.Save();

            return Ok();
        }

        [HttpPost("UpdateSubTravelPrefrence")]
        public IActionResult UpdateSubTravelPrefrence(EditSubtravelPrefrenceRequestObject editSubTravelPrefrenceRequestObj)
        {


            var subTravelPrefrenceObj = _mapper.Map<SubTravelPrefrence>(editSubTravelPrefrenceRequestObj.SubTravelPrefrenceDtoObj);


            _unitOfWork
                .SubTravelPrefrenceRepository
                .Update(subTravelPrefrenceObj);

            return Ok();
        }




        [HttpGet("GetAllSubTravelPrefrence")]
        public IActionResult GetAllSubTravelPrefrences()
        {
            var SubtravelPrefrences = _unitOfWork
                 .SubTravelPrefrenceRepository
                 .GetAll(null,null, "TravelPrefrence");


            var SubtravelPrefrencesDtos = new List<SubTravelPrefrenceDto>();


            foreach (var item in SubtravelPrefrences)
            {
                var subTravelPrefrenceDto = _mapper.Map<SubTravelPrefrenceDto>(item);
                SubtravelPrefrencesDtos.Add(subTravelPrefrenceDto);
            }



            return Ok(SubtravelPrefrencesDtos);

        }






        [HttpGet("GetSubTravelPrefrenceById")]

        public IActionResult GetSubTravelPrefrences(int id)
        {
           var SubtravelPrefrences= _unitOfWork
                .SubTravelPrefrenceRepository
                .GetAll(t => t.TravelPrefrenceId == id);


            var SubtravelPrefrencesDtos = new List<SubTravelPrefrenceDto>();


            foreach (var item in SubtravelPrefrences)
            {
                var subTravelPrefrenceDto = _mapper.Map<SubTravelPrefrenceDto>(item);
                SubtravelPrefrencesDtos.Add(subTravelPrefrenceDto);
            }



            return Ok(SubtravelPrefrencesDtos);

        }



        [HttpGet("GetIndiSubTravelPrefrenceById")]

        public IActionResult GetSubTravelPrefrenceById(int id)
        {
            var SubtravelPrefrence = _unitOfWork
                 .SubTravelPrefrenceRepository.Get(id);


            if (SubtravelPrefrence == null)
            {
                return BadRequest();
            }




             var subTravelPrefrenceDto = _mapper.Map<SubTravelPrefrenceDto>(SubtravelPrefrence);



            return Ok(subTravelPrefrenceDto);

        }




    }
}
