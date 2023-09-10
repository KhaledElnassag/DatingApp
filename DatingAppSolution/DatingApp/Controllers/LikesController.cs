using AutoMapper;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using DatingApp.DTO;
using DatingApp.Errors;
using DatingApp.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingApp.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IGenericRepository<UserLike> _LikeRepo;
        private readonly UserManager<AppUser> _UserManager;
        private readonly IMapper _mapper;

        public LikesController(IGenericRepository<UserLike> likeRepo, UserManager<AppUser> userManager,IMapper mapper)
        {
            _LikeRepo = likeRepo;
            _UserManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> Addlike(int userId)
        {
            var CurrUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appuser = await _UserManager.FindByNameAsync(CurrUser);
            if (appuser is null) return NotFound(new ErrorResponse(404));
            appuser.LikedUsers.Add(new UserLike() { TargeUserId = userId });
           var res=await _UserManager.UpdateAsync(appuser);
            if (!res.Succeeded) return BadRequest(new ErrorResponse(400));
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikedUsers()
        {
            var CurrUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appuser = await _UserManager.FindByNameAsync(CurrUser);
            var likeSpec = new LikesSpec(appuser.Id);
            var LikedUsers = await _LikeRepo.GetAllWithSpecAsync(likeSpec);
            var likesU = LikedUsers.Select(T => T.TargetUser);
            var likesUserDto = _mapper.Map<IEnumerable<LikeDto>>(likesU);
            return Ok(likesUserDto);

        }

    }
}
