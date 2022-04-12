using Microsoft.EntityFrameworkCore;
using RideShare.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RideShare.DataAcess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<PhoneVerification> PhoneVerifications { get; set; }
        public DbSet<TravelPrefrence> TravelPrefrences { get; set; }
        public DbSet<SubTravelPrefrence> SubTravelPrefrences { get; set; }
        public DbSet<UserTravellPrefrences> UserTravellPrefrences { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Ride> Ride { get; set; }
        public DbSet<RideIntermediatePosition> RideIntermediatePositions { get; set; }
        public DbSet<RideShareOffer> RideShareOffers { get; set; }
    }
}
