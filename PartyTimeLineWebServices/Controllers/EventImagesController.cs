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
using PartyTimeLineWebServices.Models;

namespace PartyTimeLineWebServices.Controllers
{
    public class EventImagesController : ApiController
    {
        private EventImagesContext db = new EventImagesContext();

        // GET: api/EventImages
        public IQueryable<EventImage> GetEventImages()
        {
            return db.EventImages;
        }

        // GET: api/EventImages/5
        [ResponseType(typeof(EventImage))]
        public IHttpActionResult GetEventImage(int id)
        {
            EventImage eventImage = db.EventImages.Find(id);
            if (eventImage == null)
            {
                return NotFound();
            }

            return Ok(eventImage);
        }

        // PUT: api/EventImages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEventImage(int id, EventImage eventImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventImage.Id)
            {
                return BadRequest();
            }

            db.Entry(eventImage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventImageExists(id))
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

        // POST: api/EventImages
        [ResponseType(typeof(EventImage))]
        public IHttpActionResult PostEventImage(EventImage eventImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventImages.Add(eventImage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = eventImage.Id }, eventImage);
        }

        // DELETE: api/EventImages/5
        [ResponseType(typeof(EventImage))]
        public IHttpActionResult DeleteEventImage(int id)
        {
            EventImage eventImage = db.EventImages.Find(id);
            if (eventImage == null)
            {
                return NotFound();
            }

            db.EventImages.Remove(eventImage);
            db.SaveChanges();

            return Ok(eventImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventImageExists(int id)
        {
            return db.EventImages.Count(e => e.Id == id) > 0;
        }
    }
}