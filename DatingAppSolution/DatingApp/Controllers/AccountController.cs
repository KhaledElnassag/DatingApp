using AutoMapper;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using DatingApp.DTO;
using DatingApp.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public AccountController(UserManager<AppUser>userManager,SignInManager<AppUser>signInManager,
            IMapper mapper,ITokenServices tokenServices)
        {
            _UserManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (UserExist(registerDto.Username.ToLower()).Result.Value) return BadRequest(new ErrorResponse(400,"User is taken"));
            var user = _mapper.Map<AppUser>(registerDto);
            var result=  await _UserManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded) return BadRequest(new ErrorResponse(400));
            return Ok(new UserDto()
            {
                Username=user.UserName,
                Token=_tokenServices.GenerateUserTokenAsync(user),
                KnownAs=user.KnownAs
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _UserManager.Users.Include(U => U.Photos).FirstOrDefaultAsync(U => U.UserName == loginDto.Username.ToLower());
            if (user is null) return Unauthorized(new ErrorResponse(401, "Username or password not valid"));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded) return Unauthorized(new ErrorResponse(401, "Username or password not valid"));
            return Ok(new UserDto()
            {
                Username = user.UserName,
                Token = _tokenServices.GenerateUserTokenAsync(user),
                PhotoUrl = user.Photos?.FirstOrDefault(P => P.IsMain)?.Url,
                 KnownAs=user.KnownAs,
                 Gender=user.Gender
            }); ;

        }

        [ApiExplorerSettings(IgnoreApi =true)]
        [HttpGet]
        public async Task<ActionResult<bool>> UserExist(string? username)
        {
            return await _UserManager.FindByNameAsync(username) is not null;
        }
    }
}
