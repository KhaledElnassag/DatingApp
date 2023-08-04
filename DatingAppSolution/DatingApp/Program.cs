using DatingApp.Repository.DataBase.UserContext;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddDbContext<DataContext>(opion =>
            {
                opion.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var Context = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                await Context.Database.MigrateAsync();
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

            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}