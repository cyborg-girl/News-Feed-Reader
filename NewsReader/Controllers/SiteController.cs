using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsReader.Models;

namespace NewsReader.Controllers
{
    public class SiteController : Controller
    {
        private SiteDBContext db = new SiteDBContext();

        // GET: /Site/
        public ActionResult Index()
        {
            return View(db.SiteDB.ToList());
        }

        // GET: /Site/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLink sitelink = db.SiteDB.Find(id);
            if (sitelink == null)
            {
                return HttpNotFound();
            }
            return View(sitelink);
        }

        // GET: /Site/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Site/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Title,Link")] SiteLink sitelink)
        {
            if (ModelState.IsValid)
            {
                db.SiteDB.Add(sitelink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sitelink);
        }

        // GET: /Site/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLink sitelink = db.SiteDB.Find(id);
            if (sitelink == null)
            {
                return HttpNotFound();
            }
            return View(sitelink);
        }

        // POST: /Site/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Title,Link")] SiteLink sitelink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sitelink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sitelink);
        }

        // GET: /Site/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLink sitelink = db.SiteDB.Find(id);
            if (sitelink == null)
            {
                return HttpNotFound();
            }
            return View(sitelink);
        }

        // POST: /Site/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteLink sitelink = db.SiteDB.Find(id);
            db.SiteDB.Remove(sitelink);
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
