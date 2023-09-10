using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using DatingApp.Extension;
using DatingApp.MiddleWares;
using DatingApp.Profiles;
using DatingApp.Repository.DataBase.DataSeeding;
using DatingApp.Repository.DataBase.UserContext;
using DatingApp.Repository.Repository;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configuration
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAppConfiguration(builder.Configuration);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddCors();
            #endregion

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var Context = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                await Context.Database.MigrateAsync();
                var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                await UserSeedData.SeedUsersAsync(usermanager);
            }
            catch (Exception ex)
            {
                var loggerFact = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFact.CreateLogger<Program>();
                logger.LogError("", ex.Message);
            }
            // Configure the HTTP request pipeline.
            #region MiddleWare
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            //app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}