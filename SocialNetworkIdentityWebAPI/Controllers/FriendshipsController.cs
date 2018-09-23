using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Data.Contexts;
using Data.Repositories.FriendshipRepositories;
using Data.Repositories.ProfileRepositories;
using DomainModel.Entities;
using DomainService;

namespace SocialNetworkIdentityWebAPI.Controllers
{
    public class FriendshipsController : ApiController
    {
        private SocialNetworkContext db;
        private FriendshipService _friendshipService;

        public FriendshipsController()
        {
            db = new SocialNetworkContext();
            _friendshipService = new FriendshipService(
                new ProfileEntityFrameworkRepository(db), 
                new FriendshipEntityFrameworkRepository(db)
                );
    }

        // GET: api/Friendships
        public IEnumerable<Friendship> GetFriendships()
        {
            return db.Friendships;
        }

        // GET: api/Friendships/5
        [ResponseType(typeof(IEnumerable<Profile>))]
        public IHttpActionResult GetFriendsOf(Guid id)
        {
            IEnumerable<Profile> friends = _friendshipService.GetFriendsOf(id);
            if (friends == null)
            {
                return NotFound();
            }

            return Ok(friends);
        }

        // PUT: api/Friendships/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFriendship(Guid id, Friendship friendship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != friendship.Id)
            {
                return BadRequest();
            }

            db.Entry(friendship).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendshipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Friendships
        [ResponseType(typeof(Friendship))]
        public IHttpActionResult PostFriendship(Friendship friendship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _friendshipService.CreateFriendship(friendship.ProfileA.Id, friendship.ProfileB.Id);

            return CreatedAtRoute("DefaultApi", new { id = friendship.Id }, friendship);
        }

        // DELETE: api/Friendships/5
        public IHttpActionResult DeleteFriendship(Guid profileId, Guid exFriendId)
        {
            _friendshipService.RemoveFriendship(profileId, exFriendId);

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FriendshipExists(Guid id)
        {
            return db.Friendships.Count(e => e.Id == id) > 0;
        }
    }
}