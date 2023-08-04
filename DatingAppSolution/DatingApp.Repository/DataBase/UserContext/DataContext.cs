﻿using DatingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Repository.DataBase.UserContext
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options) 
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}