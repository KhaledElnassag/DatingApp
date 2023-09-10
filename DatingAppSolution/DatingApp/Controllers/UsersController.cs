using AutoMapper;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using DatingApp.DTO;
using DatingApp.Errors;
using DatingApp.Helper;
using DatingApp.Repository.DataBase.UserContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace DatingApp.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IGenericRepository<AppUser> _UserRepo;
        private readonly UserManager<AppUser> _UserManager;
        private readonly IGenericRepository<AppUser> _userRepo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(UserManager<AppUser> userManager,IGenericRepository<AppUser> userRepo, IMapper mapper, IPhotoService photoService)
        {
            _UserManager = userManager;
            _userRepo = userRepo;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<MembersDto>>> GetUsers([FromQuery]PaginationParams param)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            param.CurrentName = usernam;
            var Users = await _userRepo.GetAllWithSpecAsync(new UserSpec(param));
            var count = await _userRepo.GetConutWithSpecAsync(new UserSpec(param));
            var users = _mapper.Map<IEnumerable<AppUser>, IEnumerable<MembersDto>>(Users);

            return Ok(new Pagination<MembersDto>(param.PageNumber,param.PageSize,count,users));
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<MembersDto>> GetUser(string username)
        {
            var user = await _UserManager.Users.Include(U => U.Photos).FirstOrDefaultAsync(U => U.UserName == username.ToLower());
            var User = _mapper.Map<AppUser, MembersDto>(user);
            return User is null ? NotFound(new ErrorResponse(404)) : Ok(User);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appuser = await _UserManager.FindByNameAsync(usernam);
            if (appuser is null) return NotFound(new ErrorResponse(404));
            _mapper.Map(memberUpdateDto, appuser);
            var result = await _UserManager.UpdateAsync(appuser);
            if (!result.Succeeded) return BadRequest(new ErrorResponse(400));
            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile File)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appuser = await _UserManager.Users.Include(U => U.Photos).FirstOrDefaultAsync(U => U.UserName == usernam.ToLower());
            if (appuser is null) return NotFound(new ErrorResponse(404));
            var result = await _photoService.AddPhotoAsync(File);
            if (result.Error != null) return BadRequest(new ErrorResponse(400));
            var photo = new Photo()
            {
                PublicId = result.PublicId,
                Url = result.SecureUrl.AbsoluteUri
            };
            if (appuser.Photos.Count() == 0) photo.IsMain = true;
            appuser.Photos.Add(photo);
            var res = await _UserManager.UpdateAsync(appuser);
            if (!res.Succeeded) return BadRequest(new ErrorResponse(400));

            return CreatedAtAction(nameof(GetUser), new { username = appuser.UserName }, _mapper.Map<PhotoDto>(photo));
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainhoto(int photoId)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appuser = await _UserManager.Users.Include(U => U.Photos).FirstOrDefaultAsync(U => U.UserName == usernam.ToLower());
            if (appuser is null) return NotFound(new ErrorResponse(404));
            var photo = appuser.Photos.FirstOrDefault(P => P.Id == photoId);
            if (photo == null) return NotFound(new ErrorResponse(404));
            if (!photo.IsMain)
            {
                var MainPhoto = appuser.Photos.FirstOrDefault(P => P.IsMain);
                if (MainPhoto == null) return NotFound(new ErrorResponse(404));
                MainPhoto.IsMain = false;
                photo.IsMain = true;
                var res = await _UserManager.UpdateAsync(appuser);
                if (!res.Succeeded) return BadRequest(new ErrorResponse(400));
            }
            return NoContent();
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appuser = await _UserManager.Users.Include(U => U.Photos).FirstOrDefaultAsync(U => U.UserName == usernam.ToLower());
            if (appuser is null) return NotFound(new ErrorResponse(404));
            var photo = appuser.Photos.FirstOrDefault(P => P.Id == photoId);
            if (photo == null) return NotFound(new ErrorResponse(404));
            if (photo.IsMain) return BadRequest("You can't delete your main photo");
             appuser.Photos.Remove(photo);
            var res = await _UserManager.UpdateAsync(appuser);
            if (!res.Succeeded) return BadRequest(new ErrorResponse(400));
            return NoContent();
        }
    }
}
