using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;
using DatingApp.Core.Interfaces;
using DatingApp.Errors;
using DatingApp.Helper;
using DatingApp.Repository.DataBase.UserContext;
using DatingApp.Repository.Repository;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingApp.Extension
{
    public static class IdentityServices
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection Services,IConfiguration Configuration)
        {
            Services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            Services.Configure<ApiBehaviorOptions>(op =>
            {
                op.InvalidModelStateResponseFactory = (action) =>
                {
                    var errors = action.ModelState.Where(M => M.Value.Errors.Count() > 0).SelectMany(M => M.Value.Errors)
                    .Select(E => E.ErrorMessage);
                    return new BadRequestObjectResult(new ApiValidationErrors() { Errors = errors });
                };
            });
            Services.AddDbContext<DataContext>(opion =>
            {
                opion.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
          
            Services.AddIdentity<AppUser, IdentityRole<int>>(op =>
            {
                op.Password.RequireLowercase = true;
                op.Password.RequireUppercase = true;
                op.Password.RequireDigit = true;
                op.Password.RequireNonAlphanumeric = true;
            }).AddEntityFrameworkStores<DataContext>();

            Services.AddAuthentication(op => {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            });

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<ITokenServices, TokenServices>();
            Services.AddScoped<IPhotoService, PhotoService>();
            Services.AddScoped<LogUserActivity>();
            return Services;
        }
    }
}
