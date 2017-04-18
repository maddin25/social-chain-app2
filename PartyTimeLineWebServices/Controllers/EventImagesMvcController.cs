using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PartyTimeLineWebServices.Models;

namespace PartyTimeLineWebServices.Controllers
{
    public class EventImagesMvcController : Controller
    {
        private EventImagesContext db = new EventImagesContext();

        // GET: EventImages1
        public ActionResult Index()
        {
            return View(db.EventImages.ToList());
        }

        // GET: EventImages1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventImage eventImage = db.EventImages.Find(id);
            if (eventImage == null)
            {
                return HttpNotFound();
            }
            return View(eventImage);
        }

        // GET: EventImages1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventImages1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTaken,ShortAnnotation,LongAnnotation,URI")] EventImage eventImage)
        {
            if (ModelState.IsValid)
            {
                db.EventImages.Add(eventImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventImage);
        }

        // GET: EventImages1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventImage eventImage = db.EventImages.Find(id);
            if (eventImage == null)
            {
                return HttpNotFound();
            }
            return View(eventImage);
        }

        // POST: EventImages1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTaken,ShortAnnotation,LongAnnotation,URI")] EventImage eventImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventImage);
        }

        // GET: EventImages1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventImage eventImage = db.EventImages.Find(id);
            if (eventImage == null)
            {
                return HttpNotFound();
            }
            return View(eventImage);
        }

        // POST: EventImages1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventImage eventImage = db.EventImages.Find(id);
            db.EventImages.Remove(eventImage);
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
