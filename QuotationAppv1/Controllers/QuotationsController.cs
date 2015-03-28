using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuotationAppv1.Models;

namespace QuotationAppv1.Controllers
{
    public class QuotationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public ActionResult Index(){
        //    var quotations = db.Quotations.Include(q => q.Category);
        //    return View(quotations);
        //}
        // GET: Quotations
        public ActionResult Index(string searchString)
        {
            

         //var authors = from Author in db.Quotations select Author;
         // ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_abc" : "";

         //  switch (sortOrder)
         //   {
         //       case "name_abc" :
         //           authors = authors.OrderBy(a => a.Author); 
         //           break;
         //       case "name_desc" :
         //           authors = authors.OrderByDescending(a => a.Author);
         //           break;
         //      default:
         //         break; 
         //   }

           
            //return View(authors.ToList());

            //var quotations = db.Quotations.Include(q => q.Category);
            var quotations = from a in db.Quotations select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                quotations = quotations.Where(a => a.Category.Name.Contains(searchString) || a.Author.Contains(searchString) || a.Quote.Contains(searchString));
                ViewBag.ShowLink = true;
            }
            else ViewBag.ShowLink = false; 
            return View(quotations.ToList());
        
        }
  

        // GET: Quotations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation); 
        }

        // GET: Quotations/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Quotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string add, [Bind(Include = "QuotationID,Quote,Author,CategoryID,Date")] Quotation quotation)
        { 
  
            var catCheck = from a in db.Quotations select a;
            if(!String.IsNullOrEmpty(add))
            {
                catCheck = catCheck.Where(a => a.Category.Name.Equals(add));

                if (catCheck.Equals(false))
                {
                    quotation.Category.Name = add;
 
                }
                
            }


            quotation.Date = DateTime.Now; 

            if (ModelState.IsValid)
            {   
                db.Quotations.Add(quotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", quotation.CategoryID);
            return View(quotation);
        }

        // GET: Quotations/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", quotation.CategoryID);
            return View(quotation);
        }

        // POST: Quotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuotationID,Quote,Author,CategoryID,Date")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", quotation.CategoryID);
            return View(quotation);
        }

        // GET: Quotations/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // POST: Quotations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quotation quotation = db.Quotations.Find(id);
            db.Quotations.Remove(quotation);
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
