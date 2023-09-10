using DatingApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Repository.DataBase.UserContext
{
    public class DataContext:IdentityDbContext<AppUser, IdentityRole<int>,int>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserLike>().HasKey(UL => new { UL.SourceUserId, UL.TargeUserId });

            builder.Entity<UserLike>().HasOne(L => L.SourceUser).WithMany(U => U.LikedUsers)
                .HasForeignKey(L => L.SourceUserId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserLike>().HasOne(L => L.TargetUser).WithMany(U => U.LikedByUsers)
                .HasForeignKey(L => L.TargeUserId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Messages>().HasOne(m => m.Sender).WithMany(U => U.MessagesSent)
               .HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Messages>().HasOne(m => m.Recipient).WithMany(U => U.MessagesRecieved)
                .HasForeignKey(m => m.RecipientId).OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Messages> Messages { get; set; }
    }
}
