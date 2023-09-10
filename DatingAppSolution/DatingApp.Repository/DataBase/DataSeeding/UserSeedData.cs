using DatingApp.Core.Entities;
using DatingApp.Repository.DataBase.UserContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp.Repository.DataBase.DataSeeding
{
    public static class UserSeedData
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var json = File.ReadAllText("../DatingApp.Repository/DataBase/DataSeeding/UserSeedData.json");
                var users = JsonSerializer.Deserialize < List<AppUser>>(json);
                foreach (var user in users)
                {
                    user.UserName= user.UserName.ToLower();
                   await userManager.CreateAsync(user, "Pa$$w0rd");
                }
               
            }
        }
    }
}
