using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobManager.DAL;
using JobManager.Models;

namespace JobManager.Controllers
{
    public class ConfigurationController : Controller
    {
        private ConfigContext db = new ConfigContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ListServers()
        {
            return PartialView(db.ServerConfiguration.ToList());
        }

        public ActionResult CreateServer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateServer([Bind(Include = "Id,ServerName,AuthenticationType,UserName,Password")] ServerConfig serverConfig)
        {
            if (ModelState.IsValid)
            {
                db.ServerConfiguration.Add(serverConfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serverConfig);
        }

        public ActionResult EditServer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerConfig serverConfig = db.ServerConfiguration.Find(id);
            if (serverConfig == null)
            {
                return HttpNotFound();
            }
            return View(serverConfig);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditServer([Bind(Include = "Id,ServerName,AuthenticationType,UserName,Password")] ServerConfig serverConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serverConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serverConfig);
        }

        public ActionResult DeleteServer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerConfig serverConfig = db.ServerConfiguration.Find(id);
            if (serverConfig == null)
            {
                return HttpNotFound();
            }
            return View(serverConfig);
        }

        [HttpPost, ActionName("DeleteServer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteServerConfirmed(int id)
        {
            ServerConfig serverConfig = db.ServerConfiguration.Find(id);
            db.ServerConfiguration.Remove(serverConfig);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _ListCategories()
        {
            return PartialView(db.EditableCategories.ToList());
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "Id,CategoryName,Editable")] EditableCategories editableCategories)
        {
            if (ModelState.IsValid)
            {
                db.EditableCategories.Add(editableCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editableCategories);
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditableCategories editableCategories = db.EditableCategories.Find(id);
            if (editableCategories == null)
            {
                return HttpNotFound();
            }
            return View(editableCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "Id,CategoryName,Editable")] EditableCategories editableCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(editableCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editableCategories);
        }

        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditableCategories editableCategories = db.EditableCategories.Find(id);
            if (editableCategories == null)
            {
                return HttpNotFound();
            }
            return View(editableCategories);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirmed(int id)
        {
            EditableCategories editableCategories = db.EditableCategories.Find(id);
            db.EditableCategories.Remove(editableCategories);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _ListActivity()
        {
            return PartialView(db.Activity.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
