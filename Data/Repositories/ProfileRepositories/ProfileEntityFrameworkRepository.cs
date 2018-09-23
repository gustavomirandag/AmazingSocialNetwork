using Data.Contexts;
using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.ProfileRepositories
{
    public class ProfileEntityFrameworkRepository : EntityFrameworkRepositoryBase<Profile>, IProfileRepository
    {
        public ProfileEntityFrameworkRepository(SocialNetworkContext context)
            :base(context)
        {
        }
    }
}
