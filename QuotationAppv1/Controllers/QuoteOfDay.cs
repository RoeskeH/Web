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
using QuotationAppv1.Models;


namespace QuotationAppv1.Controllers
{
    public class QuoteOfDayController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/QuoteOfDay
        public IQueryable GetQuotations()
        {
            return db.Quotations.Select(Quote => new{Quote = Quote.Quote, Author = Quote.Author});
        }

        [Route("GetDayQuote")]
        [HttpGet]
        public IHttpActionResult GetDayQuote()
        {

            var number = db.Quotations.Count();
            Random rnd = new Random();
            int day = rnd.Next(number);
            IHttpActionResult daily = GetQuotation(day);
            if (daily == null)
            {
                day = rnd.Next(number);
                daily = GetQuotation(day);
            }
            return daily; 
        }

        // GET: api/QuoteOfDay/5
        [ResponseType(typeof(QuoteOfDayController))]
        public IHttpActionResult GetQuotation(int id)
        {
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return NotFound();
            }
            return Ok(new {Quote = quotation.Quote, Author = quotation.Author});
            //return Ok(quotation);
        }

        // PUT: api/QuoteOfDay/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuotation(int id, Quotation quotation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quotation.QuotationID)
            {
                return BadRequest();
            }

            db.Entry(quotation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuotationExists(id))
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

        // POST: api/QuoteOfDay
        [ResponseType(typeof(QuoteOfDayController))]
        public IHttpActionResult PostQuotation(Quotation quotation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotations.Add(quotation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = quotation.QuotationID }, quotation);
        }

        // DELETE: api/QuoteOfDay/5
        [ResponseType(typeof(QuoteOfDayController))]
        public IHttpActionResult DeleteQuotation(int id)
        {
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return NotFound();
            }

            db.Quotations.Remove(quotation);
            db.SaveChanges();

            return Ok(quotation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuotationExists(int id)
        {
            return db.Quotations.Count(e => e.QuotationID == id) > 0;
        }
    }
}