using Google.Common.Geometry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models;
using RideShare.Models.Models.Dtos;
using RideShare.Utilities.Helpers;
using RideShare.Utilities.RequestObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RideShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RideController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpPost("CreateRide")]
        public IActionResult CreateRide(RideDto rideDto)
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




            var ride = new Ride();
            ride.StartLocationName = rideDto.StartPosition.Name;
            ride.StartLocationLatitude = rideDto.StartPosition.Lat;
            ride.StartLocationLongitude = rideDto.StartPosition.Lon;
            ride.EndLocationName = rideDto.EndPosition.Name;
            ride.EndLocationLatitude = rideDto.EndPosition.Lat;
            ride.EndLocationLongitude = rideDto.EndPosition.Lon;
            ride.DistanceinMeter = (float)rideDto.RideDistance;
            ride.DateTimeOfRide = rideDto.RideDateTime;
            string jsonString = JsonSerializer.Serialize(rideDto.RidePath);
            ride.Path = jsonString;
            ride.Price = rideDto.Price;
            ride.Note = rideDto.Note;
            ride.NumberofPassenger = rideDto.NumberofPassenger;
            ride.VehicleId = rideDto.VehicleId;
            ride.RouteVia = rideDto.RideVia;
            ride.UserId = user.Id;

            _unitOfWork.RideRepository.Add(ride);
            _unitOfWork.Save();

            foreach (var intermediateRide in rideDto.IntermediatePositions)
            {
                var intermediateRidePosition = new RideIntermediatePosition();
                intermediateRidePosition.RideId = ride.Id;
                intermediateRidePosition.PositionLatitude = Decimal.Parse(intermediateRide.Lat);
                intermediateRidePosition.PositionLongitude = Decimal.Parse(intermediateRide.Lon);
                intermediateRidePosition.PositionName = intermediateRide.Name;
                _unitOfWork.RideIntermediatePositionRepository.Add(intermediateRidePosition);
            }
            _unitOfWork.Save();

            return Ok(rideDto);
        }


        [AllowAnonymous]
        [HttpPost("CalculatePathDistance")]

        public IActionResult PathDistance(RidePathRequestObject ridePathRequestObject)
        {
            var pathlatlongs = JsonSerializer.Deserialize<List<List<decimal>>>(ridePathRequestObject.Path);

            var cells = new List<S2Point>();


            var listLength = pathlatlongs.Count();
            int index = 0;

            double totalDistance = 0;

            foreach (var latlong in pathlatlongs)
            {
                var latlng = S2LatLng.FromDegrees((double)latlong[0], (double)latlong[1]);//Indore
                var point = latlng.ToPoint();
                var npoint = S2Point.Normalize(point);

                if (index < listLength - 1)
                {

                    var nextLatLng = S2LatLng.FromDegrees((double)pathlatlongs[index+1][0], (double)pathlatlongs[index + 1][1]);
                    var distance= latlng.GetEarthDistance(nextLatLng);
                    totalDistance = totalDistance + distance;
                }

                index++;


                cells.Add(npoint);
            }

            return Ok(new {distance=totalDistance });
        }








        [HttpPost("EditRide")]
        public IActionResult EditBasicRideInfo(RideDto rideDto)
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


            var ride = _unitOfWork.RideRepository.GetFirstOrDefault(r => r.Id == rideDto.Id   && r.UserId == user.Id, "IntermediatePositions");


            if(ride==null)
            {
                return BadRequest();
            }


            ride.StartLocationName = rideDto.StartPosition.Name;
            ride.EndLocationName = rideDto.EndPosition.Name;
            ride.DateTimeOfRide = rideDto.RideDateTime;
            ride.NumberofPassenger = rideDto.NumberofPassenger;
            ride.Price = rideDto.Price;
            ride.Note = rideDto.Note;
            ride.RouteVia = rideDto.RideVia;
            ride.VehicleId = rideDto.VehicleId;
            var index = 0;
            foreach (var item in ride.IntermediatePositions)
            {
                item.PositionName = rideDto.IntermediatePositions[index].Name;
                index++;
            }


            _unitOfWork.RideRepository.Update(ride);







            //var ride = new Ride();
            //ride.StartLocationName = rideDto.StartPosition.Name;
            //ride.StartLocationLatitude = rideDto.StartPosition.Lat;
            //ride.StartLocationLongitude = rideDto.StartPosition.Lon;
            //ride.EndLocationName = rideDto.EndPosition.Name;
            //ride.EndLocationLatitude = rideDto.EndPosition.Lat;
            //ride.EndLocationLongitude = rideDto.EndPosition.Lon;
            //ride.DistanceinMeter = (float)rideDto.RideDistance;
            //ride.DateTimeOfRide = rideDto.RideDateTime;
            //string jsonString = JsonSerializer.Serialize(rideDto.RidePath);
            //ride.Path = jsonString;
            //ride.Price = rideDto.Price;
            //ride.Note = rideDto.Note;
            //ride.NumberofPassenger = rideDto.NumberofPassenger;
            //ride.VehicleId = rideDto.VehicleId;
            //ride.RouteVia = rideDto.RideVia;
            //ride.UserId = user.Id;

            //_unitOfWork.RideRepository.Add(ride);
            //_unitOfWork.Save();

            //foreach (var intermediateRide in rideDto.IntermediatePositions)
            //{
            //    var intermediateRidePosition = new RideIntermediatePosition();
            //    intermediateRidePosition.RideId = ride.Id;
            //    intermediateRidePosition.PositionLatitude = Decimal.Parse(intermediateRide.Lat);
            //    intermediateRidePosition.PositionLongitude = Decimal.Parse(intermediateRide.Lon);
            //    intermediateRidePosition.PositionName = intermediateRide.Name;
            //    _unitOfWork.RideIntermediatePositionRepository.Add(intermediateRidePosition);
            //}
            //_unitOfWork.Save();

            return Ok();
        }

        [HttpPost("EditAdvanceRideInfo")]

        public IActionResult EditAdvanceRideInfo(RideDto rideDto)
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


            var ride = _unitOfWork.RideRepository.GetFirstOrDefault(r => r.Id == rideDto.Id && r.UserId == user.Id, "IntermediatePositions");


            if (ride == null)
            {
                return BadRequest();
            }
            ride.StartLocationName = rideDto.StartPosition.Name;
            ride.StartLocationLatitude = rideDto.StartPosition.Lat;
            ride.StartLocationLongitude = rideDto.StartPosition.Lon;
            ride.EndLocationName = rideDto.EndPosition.Name;
            ride.EndLocationLatitude = rideDto.EndPosition.Lat;
            ride.EndLocationLongitude = rideDto.EndPosition.Lon;
            ride.DistanceinMeter = (float)rideDto.RideDistance;
            string jsonString = JsonSerializer.Serialize(rideDto.RidePath);
            ride.Path = jsonString;
            ride.RouteVia = rideDto.RideVia;



            foreach (var item in ride.IntermediatePositions)
            {
                _unitOfWork.RideIntermediatePositionRepository.Remove(item);
            }
            _unitOfWork.Save();

            foreach (var item in rideDto.IntermediatePositions)
            {
                var rideIntermediatePosition = new RideIntermediatePosition();
                rideIntermediatePosition.PositionLatitude = Decimal.Parse(item.Lat);
                rideIntermediatePosition.PositionLongitude =Decimal.Parse( item.Lon);
                rideIntermediatePosition.PositionName = item.Name;
                rideIntermediatePosition.RideId = ride.Id;
                _unitOfWork.RideIntermediatePositionRepository.Add(rideIntermediatePosition);
                _unitOfWork.Save();

            }

            _unitOfWork.RideRepository.Update(ride);


            return Ok();

        }

        [AllowAnonymous]
        [HttpPost("FindRide")]
        public IActionResult FindRide(FindRideDto findRideDto)
        {

            var ridesofDay = _unitOfWork.RideRepository.GetAll(r => (true)).ToList();
                
            var targetPointStartPosition = S2LatLng.FromDegrees(Double.Parse(findRideDto.StartPosition.Lat), Double.Parse(findRideDto.StartPosition.Lon));//Indore
            var targetPointFinalPosition = S2LatLng.FromDegrees(Double.Parse(findRideDto.EndPosition.Lat), Double.Parse(findRideDto.EndPosition.Lon));//Indore
            var targetStartpoint = targetPointStartPosition.ToPoint();
            var targetEndpoint = targetPointFinalPosition.ToPoint();
            var ntargetStartpoint = S2Point.Normalize(targetStartpoint);
            var ntargetFinalpoint = S2Point.Normalize(targetEndpoint);
            var selectedRides = new List<OverLappingRideDto>();
            foreach (var ride in ridesofDay)
            {
                var intermediatePositions = _unitOfWork.RideIntermediatePositionRepository.GetAll(r => r.RideId == ride.Id);

                var cells = new List<S2Point>();

                var pathlatlongs = JsonSerializer.Deserialize<List<List<decimal>>>(ride.Path);

                foreach (var latlong in pathlatlongs)
                {
                    var latlng = S2LatLng.FromDegrees((double)latlong[0], (double)latlong[1]);//Indore
                    var point = latlng.ToPoint();
                    var npoint = S2Point.Normalize(point);
                    cells.Add(npoint);
                }
                var edge = new S2Polyline(cells);
                var nearestEdgeIndexNearStartingPoint = edge.GetNearestEdgeIndex(ntargetStartpoint);
                var nearestEdgeIndexNearEndingPoint = edge.GetNearestEdgeIndex(ntargetFinalpoint);


                // distance between points 


                // distance form initial point 


                var nearestEdgeInPathInitialPosition = S2LatLng.FromDegrees((double)pathlatlongs[nearestEdgeIndexNearStartingPoint][0], (double)pathlatlongs[nearestEdgeIndexNearStartingPoint][1]);

                var nearestEdgeInPathFinalPosition = S2LatLng.FromDegrees((double)pathlatlongs[nearestEdgeIndexNearEndingPoint][0], (double)pathlatlongs[nearestEdgeIndexNearEndingPoint][1]);

                var idistance = nearestEdgeInPathInitialPosition.GetEarthDistance(targetPointStartPosition);
                var fdistance = nearestEdgeInPathFinalPosition.GetEarthDistance(targetPointFinalPosition);


                if (idistance > findRideDto.MaxRouteDistanceFromStartingPoint*1000 || fdistance > findRideDto.MaxRouteDistanceFromEndingPoint*1000)
                {
                    continue;
                }


                //if(nearestEdgeIndexNearEndingPoint - nearestEdgeIndexNearStartingPoint < 10)
                //{
                //    continue;
                //}
                if (nearestEdgeIndexNearEndingPoint - nearestEdgeIndexNearStartingPoint < 5)
                {
                    continue;
                }

                //if (nearestEdgeInPathFinalPosition.GetEarthDistance(targetPointFinalPosition) >50)
                //{
                //    continue;
                //}


                //var ratlam= S2LatLng.FromDegrees(Double.Parse("23.3315"), Double.Parse("75.0367"));
                //var indore = S2LatLng.FromDegrees(Double.Parse("28.7041"), Double.Parse("77.1025"));
                //var ratlamPoint = ratlam.GetEarthDistance(indore)/1000;










                //var angle = targetPointStartPosition.GetDistance(S2LatLng.FromDegrees((double)pathlatlongs[0][0], (double)pathlatlongs[0][1]));
                //var dassa = Utilities.Data.StaticData.RarthRadiusinKm * angle.Degrees;

                //



                if (nearestEdgeIndexNearStartingPoint <= nearestEdgeIndexNearEndingPoint)
                {
                    var overlappingRide = new OverLappingRideDto();
                    overlappingRide.RideId = ride.Id;
                    overlappingRide.OverLappingPathStartIndex = nearestEdgeIndexNearStartingPoint;
                    overlappingRide.OverLappingPathEndIndex = nearestEdgeIndexNearEndingPoint;
                    overlappingRide.OverLappingPath = pathlatlongs.Skip(nearestEdgeIndexNearStartingPoint-1).Take(nearestEdgeIndexNearEndingPoint - nearestEdgeIndexNearStartingPoint).ToList();
                    overlappingRide.DistanceFromInitialPosition = (int)idistance;
                    overlappingRide.DistanceFromFinalPosition = (int)fdistance;
                   
                    selectedRides.Add(overlappingRide);
                }

            }



            return Ok(selectedRides);
        }
        [HttpGet("GetMyRides")]
        public IActionResult GetMyRides()
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

            var rides = _unitOfWork.RideRepository.GetAll(r => r.UserId == user.Id).ToList();

            return Ok(rides);
        }
        [HttpGet("GetMyRide")]
        public IActionResult GetMyRide(int id)
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

            //var ride = _unitOfWork.RideRepository.GetFirstOrDefault(r => r.Id == id && r.UserId == user.Id, "IntermediatePositions,Vehicle");
            var ride = _unitOfWork.RideRepository.GetFirstOrDefault(r => r.Id == id , "IntermediatePositions,Vehicle");

            if (ride == null)
            {
                return BadRequest();
            }
         // ride.IntermediatePositions=  ride.IntermediatePositions.Select(ip => ip.Ride = null).ToList();
            return Ok(ride);
        }

        [HttpPost("GetOverlappingIndexes")]
        public IActionResult GetOverlappingIndexes(GetOverlappingIndexes getOverlappingIndexes)
        {

                var ride = _unitOfWork.RideRepository.Get(getOverlappingIndexes.RideId);
                var targetPointStartPosition = S2LatLng.FromDegrees(Double.Parse(getOverlappingIndexes.StartPosition.Lat), Double.Parse(getOverlappingIndexes.StartPosition.Lon));//Indore
                var targetPointFinalPosition = S2LatLng.FromDegrees(Double.Parse(getOverlappingIndexes.EndPosition.Lat), Double.Parse(getOverlappingIndexes.EndPosition.Lon));//Indore
                var targetStartpoint = targetPointStartPosition.ToPoint();
                var targetEndpoint = targetPointFinalPosition.ToPoint();
                var ntargetStartpoint = S2Point.Normalize(targetStartpoint);
                var ntargetFinalpoint = S2Point.Normalize(targetEndpoint);
                var selectedRide = new OverLappingRideDto();
                var intermediatePositions = _unitOfWork.RideIntermediatePositionRepository.GetAll(r => r.RideId == getOverlappingIndexes.RideId);

                var cells = new List<S2Point>();

                var pathlatlongs = JsonSerializer.Deserialize<List<List<decimal>>>(ride.Path);

                foreach (var latlong in pathlatlongs)
                {
                    var latlng = S2LatLng.FromDegrees((double)latlong[0], (double)latlong[1]);//Indore
                    var point = latlng.ToPoint();
                    var npoint = S2Point.Normalize(point);
                    cells.Add(npoint);
                }
                var edge = new S2Polyline(cells);
                var nearestEdgeIndexNearStartingPoint = edge.GetNearestEdgeIndex(ntargetStartpoint);
                var nearestEdgeIndexNearEndingPoint = edge.GetNearestEdgeIndex(ntargetFinalpoint);


                var nearestEdgeInPathInitialPosition = S2LatLng.FromDegrees((double)pathlatlongs[nearestEdgeIndexNearStartingPoint][0], (double)pathlatlongs[nearestEdgeIndexNearStartingPoint][1]);

                var nearestEdgeInPathFinalPosition = S2LatLng.FromDegrees((double)pathlatlongs[nearestEdgeIndexNearEndingPoint][0], (double)pathlatlongs[nearestEdgeIndexNearEndingPoint][1]);

                var idistance = nearestEdgeInPathInitialPosition.GetEarthDistance(targetPointStartPosition);
                var fdistance = nearestEdgeInPathFinalPosition.GetEarthDistance(targetPointFinalPosition);
              
                if (nearestEdgeIndexNearStartingPoint <= nearestEdgeIndexNearEndingPoint)
                {
                    var overlappingRide = new OverLappingRideDto();
                    overlappingRide.RideId = ride.Id;
                    overlappingRide.OverLappingPathStartIndex = nearestEdgeIndexNearStartingPoint;
                    overlappingRide.OverLappingPathEndIndex = nearestEdgeIndexNearEndingPoint;
                    overlappingRide.DistanceFromInitialPosition = (int)idistance;
                    overlappingRide.DistanceFromFinalPosition = (int)fdistance;

                    selectedRide=overlappingRide;
                return Ok(selectedRide);

            }
            return BadRequest();
        }



        [HttpPost("DeleteRide")]
        public IActionResult DeleteRide()
        {

            return Ok();
        }
    }
}
