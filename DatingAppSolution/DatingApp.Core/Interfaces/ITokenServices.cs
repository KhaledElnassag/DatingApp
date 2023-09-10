using DatingApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.Interfaces
{
    public interface ITokenServices
    {
        public string GenerateUserTokenAsync(AppUser user);
    }
}
