﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iManual.Models;
using iManual.Models.Domains;
using iManual.Models.ViewModels.MainCategory;
using iManual.Helper;
using Microsoft.AspNet.Identity;

namespace iManual.Controllers
{
    public class MainCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MainCategory
        public async Task<ActionResult> Index()
        {
            return View(await db.MainCategorys.ToListAsync());
        }

        // GET: MainCategory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory mainCategory = await db.MainCategorys.FindAsync(id);
            if (mainCategory == null)
            {
                return HttpNotFound();
            }
            return View(mainCategory);
        }

        // GET: MainCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MainCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateMainCategoryModel mainCategory)
        {
            if (ModelState.IsValid)
            {
                var model = new MainCategory();
                model.Name = mainCategory.Name.TrimNullable();
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = User.Identity.GetUserId();
                model.ModifiedDate = DateTime.Now;
                model.Active = true;

                db.MainCategorys.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mainCategory);
        }

        // GET: MainCategory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory mainCategory = await db.MainCategorys.FindAsync(id);
            if (mainCategory == null)
            {
                return HttpNotFound();
            }
            return View(mainCategory);
        }

        // POST: MainCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Active,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] MainCategory mainCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mainCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mainCategory);
        }

        // GET: MainCategory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory mainCategory = await db.MainCategorys.FindAsync(id);
            if (mainCategory == null)
            {
                return HttpNotFound();
            }
            return View(mainCategory);
        }

        // POST: MainCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MainCategory mainCategory = await db.MainCategorys.FindAsync(id);
            db.MainCategorys.Remove(mainCategory);
            await db.SaveChangesAsync();
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
