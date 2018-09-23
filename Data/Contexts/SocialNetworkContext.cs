using DomainModel.Entities;
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
        public DbSet<PhotoAlbum> PhotoAlbums { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        public SocialNetworkContext() 
            : base(Data.Properties
                  .Settings
                  .Default.DbConnectionString)
        {
            Database.Initialize(true);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
