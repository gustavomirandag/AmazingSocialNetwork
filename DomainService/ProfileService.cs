using Data.Repositories;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class ProfileService
    {
        public void CreateProfile(Profile profile)
        {
            var repository = new ProfileRepository();
            repository.Create(profile);
        }

        public Profile GetProfile(Guid id)
        {
            var repository = new ProfileRepository();
            return repository.Get(id);
        }
    }
}
