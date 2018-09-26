using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Data.Contexts;
using Data.Repositories;
using DomainModel.Entities;
using Utility;

namespace SocialNetworkWebApp.Controllers
{
    public class ProfilesController : Controller
    {
        private HttpClient _client;

        public ProfilesController()
        {
            _client = new HttpClient();
            //_client.BaseAddress = new Uri("https://socialnetworkwebapi.azurewebsites.net/");
            _client.BaseAddress = new Uri("http://localhost:57074/");
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void RegisterClientToken()
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["apiToken"].ToString());
        }

        // GET: Profiles
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            RegisterClientToken();
            return View(_client.GetAsync("api/profiles")
                .Result.Content
                .ReadAsAsync<IEnumerable<Profile>>().Result);
        }

        // GET: Profiles/Details/5
        public ActionResult Details(Guid? id)
        {
            RegisterClientToken();
            Profile profile;
            if (id == null)
            {
                if (Session["userEmail"] == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //Obtem Profile pelo EMAIL
                profile = _client.GetAsync("api/profiles/" + Session["userEmail"].ToString().EncodeBase64())
                    .Result.Content.ReadAsAsync<Profile>().Result;
                id = profile.Id;
            }
            else
            {
                //Obtem Profile pelo Id
                profile = _client.GetAsync("api/profiles/" + id)
                    .Result.Content.ReadAsAsync<Profile>().Result;

                //FOLLOWS (pessoas que sigo ou posso seguir)
                //Verifico se não estou olhando o meu próprio perfil
                if (profile.Email != Session["userEmail"].ToString())
                {
                    //Pego o MEU perfil (MY PROFILE)
                    Profile myProfile = _client.GetAsync("api/profiles/" + Session["userEmail"].ToString().EncodeBase64())
                    .Result.Content.ReadAsAsync<Profile>().Result;

                    //Verifico se já sou amigo dessa pessoa sigo essa pessoa
                    IEnumerable<Profile> friends = _client.GetAsync("api/Friendships/" + myProfile.Id)
                    .Result.Content.ReadAsAsync<IEnumerable<Profile>>().Result;
                    bool isMyFriend = friends.Any(p => p.Id == profile.Id);
                    ViewBag.isMyFriend = isMyFriend;
                }
            }

            //Apresento os amigos independente de ser eu ou outro perfil
            IEnumerable<Profile> profileFriends = _client.GetAsync("api/Friendships/" + id)
                .Result.Content.ReadAsAsync<IEnumerable<Profile>>().Result;
            ViewBag.Friends = profileFriends;

            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            RegisterClientToken();
            return View();
        }

        public async Task<ActionResult> CreateFriendshipWith(Guid id)
        {
            RegisterClientToken();

            //Pego o MEU perfil (MY PROFILE)
            Profile myProfile = _client.GetAsync("api/profiles/" + Session["userEmail"].ToString().EncodeBase64())
            .Result.Content.ReadAsAsync<Profile>().Result;

            //Obtenho referência a pessoa que será meu novo amigo
            Profile newFriend = _client.GetAsync("api/profiles/" + id)
            .Result.Content.ReadAsAsync<Profile>().Result;

            //Crio a amizade
            Friendship friendship = new Friendship();
            friendship.Id = Guid.NewGuid();
            friendship.ProfileA = myProfile;
            friendship.ProfileB = newFriend;
            await _client.PostAsJsonAsync<Friendship>("api/friendships/", friendship);

            return RedirectToAction("Details", newFriend.Id);
        }

        public async Task<ActionResult> RemoveFriendshipWith(Guid id)
        {
            RegisterClientToken();

            //Pego o MEU perfil (MY PROFILE)
            Profile myProfile = _client.GetAsync("api/profiles/" + Session["userEmail"].ToString().EncodeBase64())
            .Result.Content.ReadAsAsync<Profile>().Result;

            //Obtenho referência a pessoa que não será mais meu amigo
            Profile exFriend = _client.GetAsync("api/profiles/" + id)
            .Result.Content.ReadAsAsync<Profile>().Result;

            //Removo a amizade
            Friendship friendship = new Friendship();
            friendship.Id = Guid.NewGuid();
            friendship.ProfileA = myProfile;
            friendship.ProfileB = exFriend;
            await _client.DeleteAsync($"api/friendships?profileId={myProfile.Id}&exFriendId={exFriend.Id}");

            return RedirectToAction("Details", exFriend.Id);
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Email,Birthday")] Profile profile, HttpPostedFileBase PhotoFile)
        {
            RegisterClientToken();
            if (ModelState.IsValid)
            {
                profile.Id = Guid.NewGuid();
                //##### Upload da Foto para o Blob #####
                HttpPostedFileBase file = PhotoFile;
                var blobService = new AzureStorageService.BlobService();
                string fileUrl = await blobService.UploadImage("socialnetwork", Guid.NewGuid().ToString() + file.FileName, file.InputStream, file.ContentType);
                profile.Photo = fileUrl;
                //#######################################
                //db.Profiles.Add(profile);
                //db.SaveChanges();
                await _client.PostAsJsonAsync<Profile>("api/profiles", profile);
                return RedirectToAction("Details",profile.Id);
            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            RegisterClientToken();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = _client.GetAsync("api/profiles/" + id)
                .Result.Content.ReadAsAsync<Profile>().Result;
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Email,Birthday,Photo")] Profile profile)
        {
            RegisterClientToken();
            if (ModelState.IsValid)
            {
                await _client.PutAsJsonAsync<Profile>("api/profiles/" + profile.Id,profile);
            }
            return RedirectToAction("Details", profile.Id);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = _client.GetAsync("api/profiles/" + id)
                .Result.Content.ReadAsAsync<Profile>().Result;
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RegisterClientToken();
            Profile profile = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var deleteResult = _client.DeleteAsync("api/profiles/" + id).Result;
            if (deleteResult.IsSuccessStatusCode)
                profile = deleteResult.Content.ReadAsAsync<Profile>().Result;

            if (profile == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("SignOut", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
