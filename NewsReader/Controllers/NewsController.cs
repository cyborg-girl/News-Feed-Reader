using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsReader.Models;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsReader.Controllers
{
    public class NewsController : Controller
    {
        private NewsDBContext db = new NewsDBContext();
        private SiteDBContext sdb = new SiteDBContext();

        // GET: /News/
        //public ActionResult Index()
        //{
        //    return View(db.NewsDB.ToList());
        //}
        public ActionResult Index(string newsFeed, string searchString)
        {
            var siteList = new List<string>();

            var SiteQry = from d in db.NewsDB
                           orderby d.Site
                           select d.Site;

            siteList.AddRange(SiteQry.Distinct());
            ViewBag.newsFeed = new SelectList(siteList);

            var newsitems = from m in db.NewsDB
                         select m;

            if (!string.IsNullOrEmpty(newsFeed))
            {
                newsitems = newsitems.Where(x => x.Site == newsFeed);
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                newsitems = newsitems.Where(s => (s.Title.Contains(searchString) || s.Summary.Contains(searchString)));
            }

            return View(newsitems);
        }



        // GET: /News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsitem = db.NewsDB.Find(id);
            if (newsitem == null)
            {
                return HttpNotFound();
            }
            return View(newsitem);
        }

        // GET: /News/Create
        public ActionResult Create()
        {
            NewsItem newsitem = new NewsItem();

            // delete all items in db
            foreach (var item in db.NewsDB.ToList())
            {
                db.NewsDB.Remove(item);
                db.SaveChanges();
            }

            // save news for all subscibed sites
            foreach (var site in sdb.SiteDB.ToList())
            {
                
                // get news feed
                SyndicationFeed newsItems; 
                try
                {
                    newsItems = SyndicationFeed.Load(XmlReader.Create(site.Link));
                }
                catch
                {
                    continue;
                }

                // save news items in db
                foreach (var anews in newsItems.Items.ToList())
                {
                    
                    newsitem.Site = site.Title;
                    newsitem.Title = anews.Title.Text;
                    newsitem.Links = anews.Links[0].Uri.ToString();
                    newsitem.PublishDate = anews.PublishDate.DateTime;
                    // save only text summary
                    var textend = anews.Summary.Text.IndexOf('<');
                    if (textend >= 0)
                    {
                        newsitem.Summary = anews.Summary.Text.Substring(0, textend);
                    }
                    else
                    {
                        newsitem.Summary = anews.Summary.Text;
                    }

                    db.NewsDB.Add(newsitem);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index");
            
        }

        // POST: /News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Title,Links,PublishDate,Summary")] NewsItem newsitem)
        {
            if (ModelState.IsValid)
            {
                db.NewsDB.Add(newsitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsitem);
        }

        // GET: /News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsitem = db.NewsDB.Find(id);
            if (newsitem == null)
            {
                return HttpNotFound();
            }
            return View(newsitem);
        }

        // POST: /News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Title,Links,PublishDate,Summary")] NewsItem newsitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsitem);
        }

        // GET: /News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsitem = db.NewsDB.Find(id);
            if (newsitem == null)
            {
                return HttpNotFound();
            }
            return View(newsitem);
        }

        // POST: /News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsItem newsitem = db.NewsDB.Find(id);
            db.NewsDB.Remove(newsitem);
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
