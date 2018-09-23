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
using Data.Repositories;
using Data.Repositories.ProfileRepositories;
using DomainModel.Entities;
using Utility;

namespace SocialNetworkIdentityWebAPI.Controllers
{
    [Authorize]
    public class ProfilesController : ApiController
    {
        private SocialNetworkContext db = new SocialNetworkContext();
        private DomainService.ProfileService _profileService = new DomainService.ProfileService(new ProfileEntityFrameworkRepository(new SocialNetworkContext()));

        // GET: api/Profiles
        public IEnumerable<Profile> GetProfiles()
        {
            var profiles = _profileService.GetAllProfiles();
            return profiles;
        }

        // GET: api/Profiles/5
        [Route("api/Profiles/{id:guid}")]
        [ResponseType(typeof(Profile))]
        public IHttpActionResult GetProfile(Guid id)
        {
            Profile profile = null;
            profile = _profileService.GetProfile(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        // GET: api/Profiles/5
        [HttpGet]
        [Route("api/Profiles/{email}")]
        //[ResponseType(typeof(Profile))]
        public IHttpActionResult GetByEmail(string email)
        {
            email = email.DecodeBase64();
            Profile profile = db.Profiles.SingleOrDefault(p => p.Email == email);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        // PUT: api/Profiles/5
        [HttpPut]
        [Route("api/Profiles/{id:guid}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfile(Guid id, Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profile.Id)
            {
                return BadRequest();
            }

            _profileService.UpdateProfile(profile);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        [ResponseType(typeof(Profile))]
        public IHttpActionResult PostProfile(Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _profileService.CreateProfile(profile);

            return CreatedAtRoute("DefaultApi", new { id = profile.Id }, profile);
        }

        // DELETE: api/Profiles/5
        [Route("api/Profiles/{id:guid}")]
        [ResponseType(typeof(Profile))]
        public IHttpActionResult DeleteProfile(Guid id)
        {
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return NotFound();
            }

            db.Profiles.Remove(profile);
            db.SaveChanges();

            return Ok(profile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileExists(Guid id)
        {
            return db.Profiles.Count(e => e.Id == id) > 0;
        }
    }
}