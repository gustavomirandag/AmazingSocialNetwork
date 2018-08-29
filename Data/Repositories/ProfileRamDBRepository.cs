using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProfileRamDBRepository : IProfileRepository
    {
        private static List<Profile> profiles = new List<Profile>();

        public void Create(Profile profile)
        {
            profiles.Add(profile);
        }

        public Profile Get(Guid? id)
        {
            return profiles.SingleOrDefault(p => p.Id == id);
        }
    }
}
