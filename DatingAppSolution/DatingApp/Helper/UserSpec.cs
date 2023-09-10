using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Helper
{
    public class UserSpec:BaseSpecification<AppUser>
    {
        public UserSpec(PaginationParams specparm):base(U=>U.UserName!=specparm.CurrentName
                                                  && (string.IsNullOrEmpty(specparm.Gender)||U.Gender==specparm.Gender)
                                                   &&(U.DateOfBirth>=specparm.MinAg&&U.DateOfBirth <= specparm.MaxAg)) {
            IncludeCrietria.Add(U => U.Photos);
            ApplyPagination(specparm.PageSize, (specparm.PageNumber - 1) * specparm.PageSize);

            switch (specparm.OrderBy)
            {
                case "created" :AddOrderByDesc(U => U.Created);
                    break;
                default: AddOrderByDesc(U => U.LastActive);
                break;
            }
        }
    }
}
