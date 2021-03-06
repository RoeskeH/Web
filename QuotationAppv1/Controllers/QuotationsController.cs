﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuotationAppv1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QuotationAppv1.Controllers
{
    public class QuotationsController : Controller
    {
        private UserManager<ApplicationUser> manager; 
        private ApplicationDbContext db = new ApplicationDbContext();

        public QuotationsController(){
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            
           
        }

        
            
        
        // GET: Quotations
        public ActionResult Index(string searchString, string LogQuotes)
        {
            var baseUri = new Uri("http://localhost:4852");
            HttpClient client = new HttpClient();
            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("GetDayQuote").Result;
            if (response.IsSuccessStatusCode)
            {
                Quotation data = response.Content.ReadAsAsync<Quotation>().Result;
                string dayQuote = data.Quote +" by "+ data.Author; 
                ViewBag.DayQuote = dayQuote; 
            }
               
               var quotations = from a in db.Quotations select a;
               var userQuotes = User.Identity.GetUserId(); 


             if (!String.IsNullOrEmpty(searchString))
               {
                   quotations = Search(searchString, quotations);
                   ViewBag.ShowLink = true;
                  

               }
               else if(LogQuotes == "Show My Quotes") 
               {

                   ViewBag.ShowLink = false;
                   quotations = UserQuotes(userQuotes, quotations);
                   ViewBag.ShowQuotes = true; 

               }
             else
             {
                 ViewBag.ShowLink = false;
                 ViewBag.ShowQuotes = false; 
                 
             }
            
        return View(quotations.ToList()); 
          
        }

        private static IQueryable<Quotation> UserQuotes(string userQuotes, IQueryable<Quotation> quotations)
        {
            return quotations.Where(a => a.User.Id.Contains(userQuotes));
        }

        private static IQueryable<Quotation> Search(string searchString, IQueryable<Quotation> quotations)
        {
            return quotations.Where(a => a.Category.Name.Contains(searchString) || a.Author.Contains(searchString) || a.Quote.Contains(searchString));
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
        public ActionResult Create(string add, [Bind(Include = "QuotationID,Quote,Author,CategoryID,Date,User")] Quotation quotation)
        {
            if (!String.IsNullOrEmpty(add))
            {
                var catCheck = from a in db.Categories select a;
                catCheck = catCheck.Where(a => a.Name.Equals(add));


                if (catCheck.Count() == 0)
                {
                    Category c = new Category { Name = add };
                    db.Categories.Add(c);
                    db.SaveChanges();
                    quotation.Category = c; 
                   
                }
            }

          

            quotation.Date = DateTime.Now;
            ModelState["CategoryID"].Errors.Clear(); 

            if (ModelState.IsValid)
            {
               
                var user = manager.FindById(User.Identity.GetUserId());
                quotation.User = user; 

                db.Quotations.Add(quotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
