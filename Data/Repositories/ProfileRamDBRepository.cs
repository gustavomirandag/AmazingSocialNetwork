using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProfileRamDBRepository : IRepository<Profile>
    {
        private static List<Profile> profiles = new List<Profile>();

        public void Create(Profile profile)
        {
            profiles.Add(profile);
        }

        public void Delete(Guid id)
        {
            var orignalProfile 
                = profiles.SingleOrDefault(p => p.Id == id);
        }

        public Profile Get(Guid? id)
        {
            return profiles.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Profile> GetAll()
        {
            return profiles;
        }

        public void Update(Profile entity)
        {
            var originalProfile 
                = profiles.SingleOrDefault(p => p.Id == entity.Id);
            profiles.Remove(originalProfile);
            profiles.Add(entity);
        }
    }
}
