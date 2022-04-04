using AutoMapper;
using RideShare.Models.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;


namespace RideShare.Models.Models.Mapper
{
   public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<TravelPrefrence, TravelPrefrenceDto>().ReverseMap();
            CreateMap<SubTravelPrefrence, SubTravelPrefrenceDto>().ReverseMap();
            CreateMap<UserTravellPrefrences, UserTravelPrefrencesDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<VehicleType, VehicleTypeDto>().ReverseMap();
            CreateMap<User, UserPublicProfileDto>().ReverseMap();




        }
    }
}
