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

        // GET: Configuration
        public ActionResult Index()
        {
            return View(db.ServerConfiguration.ToList());
        }

        // GET: Configuration/Details/5
        public ActionResult Details(int? id)
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

        // GET: Configuration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Configuration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ServerName,AuthenticationType,UserName,Password")] ServerConfig serverConfig)
        {
            if (ModelState.IsValid)
            {
                db.ServerConfiguration.Add(serverConfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serverConfig);
        }

        // GET: Configuration/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Configuration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ServerName,AuthenticationType,UserName,Password")] ServerConfig serverConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serverConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serverConfig);
        }

        // GET: Configuration/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Configuration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServerConfig serverConfig = db.ServerConfiguration.Find(id);
            db.ServerConfiguration.Remove(serverConfig);
            db.SaveChanges();
            return RedirectToAction("Index");
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
