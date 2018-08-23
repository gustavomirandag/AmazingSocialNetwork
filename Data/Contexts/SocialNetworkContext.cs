﻿using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public class SocialNetworkContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public SocialNetworkContext() 
            : base(Data.Properties
                  .Settings
                  .Default.DbConnectionString)
        {
        }
    }
}
