using System;
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
using Microsoft.AspNet.Identity.Owin;

namespace iManual.Controllers
{
    public class SubCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public SubCategoriesController() { }
        public SubCategoriesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: SubCategories
        public async Task<ActionResult> Index()
        {
            var subCategorys = db.SubCategorys.Include(s => s.MainCategory);
            return View(await subCategorys.ToListAsync());
        }

        // GET: SubCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await db.SubCategorys.FindAsync(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.MainCategoryId = new SelectList(db.MainCategorys, "Id", "Name");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MainCategoryId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategorys.Add(subCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MainCategoryId = new SelectList(db.MainCategorys, "Id", "Name", subCategory.MainCategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await db.SubCategorys.FindAsync(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.MainCategoryId = new SelectList(db.MainCategorys, "Id", "Name", subCategory.MainCategoryId);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MainCategoryId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MainCategoryId = new SelectList(db.MainCategorys, "Id", "Name", subCategory.MainCategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await db.SubCategorys.FindAsync(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubCategory subCategory = await db.SubCategorys.FindAsync(id);
            db.SubCategorys.Remove(subCategory);
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
