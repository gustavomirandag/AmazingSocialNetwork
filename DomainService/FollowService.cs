using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class FollowService
    {
        private IProfileRepository _profileRepository;
        public FollowService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public void Follow(Profile follower, Profile followed)
        {
            follower.Follows.Add(followed);
            _profileRepository.Update(follower);
        }
    }
}
