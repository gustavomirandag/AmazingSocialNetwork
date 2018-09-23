using Data.Contexts;
using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.FriendshipRepositories
{
    public class FriendshipEntityFrameworkRepository : EntityFrameworkRepositoryBase<Friendship>, IFriendshipRepository
    {
        public FriendshipEntityFrameworkRepository(SocialNetworkContext context)
            :base(context)
        {
        }
    }
}
