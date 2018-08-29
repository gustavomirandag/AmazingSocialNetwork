using Data.Contexts;
using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProfileEntityFrameworkRepository : IProfileRepository
    {
        public void Create(Profile profile)
        {
            var context = new SocialNetworkContext();
            context.Profiles.Add(profile);
            context.SaveChanges();
        }

        public Profile Get(Guid? id)
        {
            if (id == null)
                return null;
            var context = new SocialNetworkContext();
            return context.Profiles
                .SingleOrDefault(x => x.Id == id);
        }
    }
}
