using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DatingApp.Helper
{
    public class MessageSpec:BaseSpecification<Messages>
    {
        public MessageSpec(int senderId,int receptientId):base(M=>(M.SenderId==senderId&& M.RecipientId == receptientId)
                                                           || (M.SenderId == receptientId && M.RecipientId == senderId)) {
            AddOrderBy(M => M.MessageSent);
            IncludeCrietria.Add(M => M.Recipient);
            IncludeCrietria.Add(M => M.Recipient.Photos);
            IncludeCrietria.Add(M => M.Sender);
            IncludeCrietria.Add(M => M.Sender.Photos);
        }

        public MessageSpec(MessageParams messageParams) 
        {
            AddOrderByDesc(M => M.MessageSent);
            
             switch(messageParams.Container)
            {
                case "Inbox" :
                    Crietria=(u => u.Recipient.UserName == messageParams.Username &&
                 u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    Crietria = (u => u.Sender.UserName == messageParams.Username &&
                    u.SenderDeleted == false);
                    break;
               default:
                    Crietria = (u => u.Recipient.UserName == messageParams.Username
                    && u.RecipientDeleted == false && u.DateRead == null);
                    break;
            };
            IncludeCrietria.Add(M => M.Recipient);
            IncludeCrietria.Add(M => M.Recipient.Photos);
            IncludeCrietria.Add(M => M.Sender);
            IncludeCrietria.Add(M => M.Sender.Photos);
        }

        public async Task<int> updateUserMessage(IEnumerable<Messages> messages,int sender,int reciver,IGenericRepository<Messages>_messageRepo)
        {
            foreach (var item in messages)
            {
                if (item.DateRead == null&&item.RecipientId== sender&&item.SenderId==reciver)
                {
                    item.DateRead = DateTime.UtcNow;
                    _messageRepo.Update(item);
                }
            }
            return await _messageRepo.Complete();
        }
    }
}
