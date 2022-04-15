using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using RideShare.Models.Models.Enums;
using RideShare.Utilities.Helpers;
using RideShare.Utilities.RequestObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideShareOfferController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RideShareOfferController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }






        [HttpGet("GetById")]

        public IActionResult GetById(int id)
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

            var rideShareOffer = _unitOfWork.RideShareOfferRepository.GetById(id);

            if (rideShareOffer == null)
            {
                return BadRequest();
            }

            return Ok(rideShareOffer);
        }

        [HttpGet("GetAllApprovedRideShareOfferids")]

        public IActionResult GetAllApprovedRides(int id)
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

            if (user == null)
            {
                return BadRequest();
            }
            var rideShareRequests = _unitOfWork.RideShareOfferRepository.GetAllApprovedRideOffers(id);

            return Ok(rideShareRequests);
        }

        [HttpGet("GetAllDisApprovedRideShareOfferids")]

        public IActionResult GetAllDisApprovedRidesOffers(int id)
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

            if (user == null)
            {
                return BadRequest();
            }
            var rideShareRequests = _unitOfWork.RideShareOfferRepository.GetAllDisApprovedRideOffers(id);

            return Ok(rideShareRequests);
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
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

            if(user==null)
            {
                return BadRequest();
            }
            var rideShareRequests = _unitOfWork.RideShareOfferRepository.GetAll(user.Id);

            return Ok(rideShareRequests);
        }


        [HttpGet("GetAllOffers")]

        public IActionResult GetAllOffers()
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

            if (user == null)
            {
                return BadRequest();
            }
            var rideShareOffers = _unitOfWork.RideShareOfferRepository.GetAllOffers(user.Id);

            return Ok(rideShareOffers);

        }


        [HttpPost("Get")]

        public IActionResult Get(GetRideShareOffer getRideShareOffer )
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

            var rideShareOffer = _unitOfWork.RideShareOfferRepository.GetFirstOrDefault(rso => rso.UserId == user.Id && rso.RideId == getRideShareOffer.RideId);

            if(rideShareOffer==null)
            {
                return BadRequest();
            }

            return Ok(rideShareOffer);
        }

        [HttpPost("Delete")]

        public IActionResult Delete(int id)
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

            var rideShareOffer = _unitOfWork.RideShareOfferRepository.GetFirstOrDefault(rso => rso.Id ==id);
            if(rideShareOffer==null)
            {
                return BadRequest();
            }
            _unitOfWork.RideShareOfferRepository.Remove(rideShareOffer.Id);
            _unitOfWork.Save();
            return Ok();

        }

        [HttpPost("Create")]

        public IActionResult Create(RideShareOffer rideShareOffer)
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
            var ride = _unitOfWork.RideRepository.GetFirstOrDefault(r => r.Id == rideShareOffer.RideId);

            if(ride==null)
            {
                return BadRequest();
            }

            if (ride.UserId == user.Id)
            {
                return BadRequest();
            }


            var isOfferAlreadycreated = _unitOfWork.RideShareOfferRepository.IsRequestForRideAlreadyCreated(rideShareOffer.RideId, rideShareOffer.UserId);

            if (isOfferAlreadycreated)
            {
                var savedRideShareOffer = _unitOfWork.RideShareOfferRepository.GetRideShareOfferForRideofUser(rideShareOffer.UserId, rideShareOffer.RideId);
                savedRideShareOffer.StartLocationName = rideShareOffer.StartLocationName;
                savedRideShareOffer.EndLocationName = rideShareOffer.EndLocationName;
                savedRideShareOffer.StartLocationLatitude = rideShareOffer.StartLocationLatitude;
                savedRideShareOffer.StartLocationLongitude = rideShareOffer.StartLocationLongitude;
                savedRideShareOffer.EndLocationLatitude = rideShareOffer.EndLocationLatitude;
                savedRideShareOffer.EndLocationLongitude = rideShareOffer.EndLocationLongitude;
                savedRideShareOffer.OfferedPrice = rideShareOffer.OfferedPrice;
                savedRideShareOffer.NumberOfPassengers = rideShareOffer.NumberOfPassengers;
                savedRideShareOffer.NotesForRideCreater = rideShareOffer.NotesForRideCreater;
                savedRideShareOffer.LastUpdated = DateTimeOffset.UtcNow;

                if (
                savedRideShareOffer.RideShareOfferStatus == Models.Models.Enums.RideShareOfferStatus.Intrested
               || savedRideShareOffer.RideShareOfferStatus == Models.Models.Enums.RideShareOfferStatus.Rejected
               || savedRideShareOffer.RideShareOfferStatus == Models.Models.Enums.RideShareOfferStatus.Viewed
               )
                {
                    savedRideShareOffer.RideShareOfferStatus = Models.Models.Enums.RideShareOfferStatus.Updated;
                }

                _unitOfWork.RideShareOfferRepository.Update(savedRideShareOffer);
                _unitOfWork.Save();
                return Ok();
            }
            rideShareOffer.CreatedDateTime = DateTimeOffset.UtcNow;

            rideShareOffer.LastUpdated = DateTimeOffset.UtcNow;

           
            rideShareOffer.RideShareOfferStatus = Models.Models.Enums.RideShareOfferStatus.Pending;
            _unitOfWork.RideShareOfferRepository.Add(rideShareOffer);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPost("GetRideShareOfferStatusText")]

        public IActionResult GetRideShareOfferStatusText(int id)
        {
            string name = null;
            try
            {
                 name = ((RideShareOfferStatus)id).ToString();

            }
            catch( Exception e)
            {
                return BadRequest();
            }
            return Ok(new {name=name });
        }

        [HttpPost("LoadRideShareOfferForReview")]
        public IActionResult LoadRideShareOfferFor(GetRideShareOffer getRideShareOffer)
        {
            _unitOfWork.RideShareOfferRepository.SetRideShareOfferForReview(getRideShareOffer.RideId);
            return Ok();
        }

        [HttpPost("SetResponseForRideShareOffer")]
        public IActionResult UpdateRideShareOfferState(RideShareOfferResponse rideshareOfferResponse)
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

            if(user==null)
            {
                return BadRequest();
            }

            var savedRideShareOffer = _unitOfWork.RideShareOfferRepository.GetFirstOrDefault(r=>r.Id== rideshareOfferResponse.RideShareOfferId, "Ride");
            
            if(savedRideShareOffer==null)
            {
                return BadRequest();
            }

            if(savedRideShareOffer.Ride.UserId != user.Id)
            {
                return BadRequest();
            }

            savedRideShareOffer.LastUpdated = DateTimeOffset.UtcNow;
            savedRideShareOffer.NotesForOfferCreator =rideshareOfferResponse.MessageForOfferCreator;
            savedRideShareOffer.RideShareOfferStatus = (RideShareOfferStatus)rideshareOfferResponse.statusId;

            _unitOfWork.RideShareOfferRepository.Update(savedRideShareOffer);

            _unitOfWork.Save();

        //    _unitOfWork.RideRepository.UpdateNumberOfPassengers(savedRideShareOffer.RideId, savedRideShareOffer.NumberOfPassengers);

            //  _unitOfWork.RideShareOfferRepository.Update(rideshareOfferResponse)

            return Ok();
        }






    }
}
