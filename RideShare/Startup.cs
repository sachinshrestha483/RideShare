using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RideShare.DataAcess;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Models.Models.Mapper;
using RideShare.Utilities.Helpers.EmailHelper;
using RideShare.Utilities.Helpers.MessageHelper;
using RideShare.Utilities.Security;

namespace RideShare
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));




            services.Configure<EmailOptions>(Configuration);

            services.AddSingleton<IEmailSender, EmailSender>();



            services.AddHttpContextAccessor();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            // we use appSettingss While Configuring the JWT Tokens"
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();// here that class is used

            var key = Encoding.ASCII.GetBytes(appSettings.JwtTokenSecret);// value of the Security Key


            services.AddAuthentication(x =>
            {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero








                };
               
            });
            ;






            services.AddAutoMapper(typeof(Mapping));

            services.AddSingleton<DataProtectionPurposeStrings>();


            services.AddSwaggerGen(options => {

                options.SwaggerDoc("ParkyOpenApiSpec"
                    , new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Parky Api",
                        Version = "1"
                    });



                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
             "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
             "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
             "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });





                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlcommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlcommentsFullPath);
            });




            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
            x => x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );


            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/ParkyOpenApiSpec/swagger.json", "ParkyApi");
                options.RoutePrefix = "";
                // now to go to launch Setting.json for making it default launch go to profiles and remove launchurl whose value is weather forecast
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
