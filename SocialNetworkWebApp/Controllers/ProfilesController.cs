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
        HttpClient _client;

        public ProfilesController()
        {
            _client = new HttpClient();
            //_client.BaseAddress = new Uri("https://socialnetworkwebapi.azurewebsites.net/");
            _client.BaseAddress = new Uri("http://localhost:57377/");
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Profiles
        public ActionResult Index()
        {
            return View(_client.GetAsync("api/profiles")
                .Result.Content
                .ReadAsAsync<IEnumerable<Profile>>().Result);
        }

        // GET: Profiles/Details/5
        public ActionResult Details(Guid? id)
        {
            Profile profile;
            if (id == null)
            {
                if (Session["userEmail"] == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //Obtem Profile pelo EMAIL
                profile = _client.GetAsync("api/profiles/" + Session["userEmail"].ToString().EncodeBase64())
                    .Result.Content.ReadAsAsync<Profile>().Result;
            }
            else
            {
                //Obtem Profile pelo Id
                profile = _client.GetAsync("api/profiles/" + id)
                    .Result.Content.ReadAsAsync<Profile>().Result;
            }

            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Email,Birthday")] Profile profile, HttpPostedFileBase PhotoFile)
        {
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
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Birthday,Photo")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _client.PutAsJsonAsync<Profile>("api/profiles",profile);
                return RedirectToAction("Index");
            }
            return View(profile);
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
