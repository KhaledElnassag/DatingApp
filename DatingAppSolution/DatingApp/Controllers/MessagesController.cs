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
using System.Reflection;
using System.Security.Claims;

namespace DatingApp.Controllers
{
    
    public class MessagesController : BaseApiController
    {
        private readonly IGenericRepository<Messages> _MessRepo;
        private readonly UserManager<AppUser> _UserManager;
        private readonly IMapper _mapper;

        public MessagesController(IGenericRepository<Messages> messRepo, UserManager<AppUser> userManager,IMapper mapper)
        {
            _MessRepo = messRepo;
            _UserManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(CreateMessageDto createMessage)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(usernam== createMessage.RecipientUsername)return BadRequest(new ErrorResponse(400,"you can't send to yourself"));
            var sender = await _UserManager.Users.Include(U => U.Photos).FirstOrDefaultAsync(U => U.UserName == usernam.ToLower());
            var recipient= await _UserManager.Users.Include(U => U.Photos)
                .FirstOrDefaultAsync(U => U.UserName == createMessage.RecipientUsername.ToLower()); ;
            var message = new Messages()
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessage.Content
            };

            await _MessRepo.AddAsync(message);
            var res = await _MessRepo.Complete();
            if(res==0) return BadRequest(new ErrorResponse(400));
            return Ok(_mapper.Map<MessageDto>(message));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
        MessageParams messageParams)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            messageParams.Username=usernam;
            var messageSpec = new MessageSpec(messageParams);
            var messages = await _MessRepo.GetAllWithSpecAsync(messageSpec);
            var MessDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return Ok(MessDto);
        }

        [HttpGet("thread/{userid}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser(int userid)
        {
            var usernam = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sender = await _UserManager.FindByNameAsync(usernam);
            var messageSpec = new MessageSpec(sender.Id, userid);
            var messages=await _MessRepo.GetAllWithSpecAsync(messageSpec);
            await messageSpec.updateUserMessage(messages,sender.Id,userid, _MessRepo);
           
            var MessDto=_mapper.Map<IEnumerable<MessageDto>>(messages);
            return Ok(MessDto);
        }
    }
}
