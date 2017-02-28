using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobManager.Data;

namespace JobManager.Controllers
{
    public class ConfigurationController : Controller
    {
        private ConfigModel db = new ConfigModel();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ListServers()
        {
            return PartialView(db.ServerConfigs.ToList());
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
                db.ServerConfigs.Add(serverConfig);
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
            ServerConfig serverConfig = db.ServerConfigs.Find(id);
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
            ServerConfig serverConfig = db.ServerConfigs.Find(id);
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
            ServerConfig serverConfig = db.ServerConfigs.Find(id);
            db.ServerConfigs.Remove(serverConfig);
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
        public ActionResult CreateCategory([Bind(Include = "Id,CategoryName,Editable")] EditableCategory EditableCategory)
        {
            if (ModelState.IsValid)
            {
                db.EditableCategories.Add(EditableCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(EditableCategory);
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditableCategory editableCategories = db.EditableCategories.Find(id);
            if (editableCategories == null)
            {
                return HttpNotFound();
            }
            return View(editableCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "Id,CategoryName,Editable")] EditableCategory editableCategories)
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
            EditableCategory editableCategories = db.EditableCategories.Find(id);
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
            EditableCategory editableCategories = db.EditableCategories.Find(id);
            db.EditableCategories.Remove(editableCategories);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _ListActivity()
        {
            return PartialView(db.ActivityLogs.ToList());
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
