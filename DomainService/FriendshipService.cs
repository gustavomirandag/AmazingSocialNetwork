using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class FriendshipService
    {
        private IProfileRepository _profileRepository;
        private IFriendshipRepository _friendshipRepository;
        public FriendshipService(IProfileRepository profileRepository, IFriendshipRepository friendshipRepository)
        {
            _profileRepository = profileRepository;
            _friendshipRepository = friendshipRepository;
        }

        public void CreateFriendship(Guid profileIdA, Guid profileIdB)
        {
            //Eu poderia incluir dois registros ao invés de apenas 1.
            //poderia dizer que o ProfileA é amigo de ProfileB
            //e que ProfileB é amigo do ProfileA
            //mas nesse exemplo estou incluindo apenas um registro
            //que representa a amizade como um todo.
            Friendship friendship = new Friendship();
            friendship.Id = Guid.NewGuid();
            friendship.ProfileA = _profileRepository.Get(profileIdA);
            friendship.ProfileB = _profileRepository.Get(profileIdB);
            _friendshipRepository.Create(friendship);
        }

        //Função que retorna quem um determinado ID segue
        public IEnumerable<Profile> GetFriendsOf(Guid id)
        {
            //Como registrei a amizade como um único registro
            //Preciso verificar se meu profile está do lado esquerdo (A)
            //ou do lado direito (B) de uma amizade

            //Aqui eu pego todas as amizades cadastradas no banco de dados
            //isso não é performático, no trabalho vocês tem que fazer via 
            //StoredProcedure/SQL
            var friendships = _friendshipRepository.GetAll();

            //Agora pego todas as amizades com o profile
            List<Profile> profileFriends = new List<Profile>();
            foreach(var friendship in friendships)
            {
                if (friendship.ProfileA.Id == id)
                    profileFriends.Add(friendship.ProfileB);
                if (friendship.ProfileB.Id == id)
                    profileFriends.Add(friendship.ProfileA);
            }

            return profileFriends;
        }

        public void RemoveFriendship(Guid profileIdA, Guid profileIdB)
        {
            var friendships = _friendshipRepository.GetAll();

            //Agora procuro a amizade entre ProfileA e ProfileB
            Friendship friendshipToBeRemoved = new Friendship();
            foreach (var friendship in friendships)
            {
                if (
                    (friendship.ProfileA.Id == profileIdA
                    && friendship.ProfileB.Id == profileIdB)
                    ||
                    (friendship.ProfileA.Id == profileIdB
                    && friendship.ProfileB.Id == profileIdA)
                   )
                {
                    friendshipToBeRemoved = friendship;
                    _friendshipRepository.Delete(friendshipToBeRemoved.Id);
                    return;
                }
            }
        }
    }
}
