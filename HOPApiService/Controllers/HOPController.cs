using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HOPApiService.Models;

namespace HOPApiService.Controllers
{
    public class HOPController : ApiController
    {
        private readonly DataContext db = new DataContext();

        // GET api/Default1
        public IEnumerable<HOP> GetHOPs()
        {
            return db.Hops.AsEnumerable();
        }

        // GET api/Default1/5
        public HOP GetHOP(int id)
        {
            var hop = db.Hops.Find(id);
            if (hop == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return hop;
        }

        // PUT api/Default1/5
        public HttpResponseMessage PutHOP(int id, HOP hop)
        {
            if (ModelState.IsValid && id == hop.Id)
            {
                db.Entry(hop).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, hop);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // POST api/Default1
        public HttpResponseMessage PostHOP(HOP hop)
        {
            if (ModelState.IsValid)
            {
                db.Hops.Add(hop);
                db.SaveChanges();

                var response = Request.CreateResponse(HttpStatusCode.Created, hop);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = hop.Id }));
                return response;
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // DELETE api/Default1/5
        public HttpResponseMessage DeleteHOP(int id)
        {
            var hop = db.Hops.Find(id);
            if (hop == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Hops.Remove(hop);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, hop);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}