using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.Entities
{
    public class AppUser:IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public string Gender { get; set; }

        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
        public ICollection<UserLike> LikedByUsers { get; set; } = new HashSet<UserLike>();
        public ICollection<UserLike> LikedUsers { get; set; } = new HashSet<UserLike>();
        public ICollection<Messages> MessagesSent { get; set; } = new HashSet<Messages>();
        public ICollection<Messages> MessagesRecieved { get; set; } = new HashSet<Messages>();
        public int Age => DateOfBirth.CalculateAge();

    }
}
