﻿using Data.Repositories;
using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class ProfileService
    {
        private IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public void CreateProfile(Profile profile)
        {
            _profileRepository.Create(profile);
        }

        public Profile GetProfile(Guid? id)
        {
            return _profileRepository.Get(id);
        }

        public IEnumerable<Profile> GetAllProfiles()
        {
            return _profileRepository.GetAll();
        }

        public void RemoveProfile(Guid id)
        {
            _profileRepository.Delete(id);
        }

        public void UpdateProfile(Profile profile)
        {
            _profileRepository.Update(profile);
        }
    }
}
