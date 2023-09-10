using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;

namespace DatingApp.Helper
{
    public class LikesSpec:BaseSpecification<UserLike>
    {
        public LikesSpec(int likeId):base(L=>L.SourceUserId==likeId)
        {
            IncludeCrietria.Add(L => L.TargetUser);
            IncludeCrietria.Add(L => L.TargetUser.Photos);
        }
    }
}
