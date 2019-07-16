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
using iManual.Models.ViewModels.SubCategoryViewModels;
using iManual.Helper;
using Microsoft.AspNet.Identity;

namespace iManual.Controllers
{
    public class SubCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public SubCategoryController() { }
        public SubCategoryController(ApplicationUserManager userManager)
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
            SubCategory subCategory = await db.SubCategorys.Include(p=>p.MainCategory).FirstOrDefaultAsync(p=>p.Id == id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            var model = new CreateSubCategoryViewModel();
            model.MainCategorys = db.MainCategorys.OrderBy(p => p.Name).Where(p=>p.Active == true).ToList();
            return View(model);
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateSubCategoryModel subCategory)
        {
            if (ModelState.IsValid)
            {
                var model = new SubCategory();
                model.Name = subCategory.Name.TrimNullable();
                model.MainCategoryId = subCategory.MainCategoryId;
                model.CreatedBy = User.Identity.GetUserId();
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Active = true;

                db.SubCategorys.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var returnModel = new CreateSubCategoryViewModel();
            returnModel.MainCategorys = db.MainCategorys.OrderBy(p => p.Name).Where(p => p.Active == true).ToList();
            return View(returnModel);
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

            var model = new EditSubCategoryViewModel();
            model.Id = subCategory.Id;
            model.Name = subCategory.Name;
            model.Active = subCategory.Active;
            model.MainCategorys = db.MainCategorys.OrderBy(p => p.Name).Where(p=>p.Active == true).ToList();
            model.MainCategory =await db.MainCategorys.SingleOrDefaultAsync(p=>p.Id == subCategory.MainCategoryId);

            return View(model);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSubCategoryModel subCategory)
        {
            if (ModelState.IsValid)
            {
                var dbSubCategory = db.SubCategorys.SingleOrDefault(p=>p.Id == subCategory.Id);
                dbSubCategory.Name = subCategory.Name.TrimNullable();
                dbSubCategory.Active = subCategory.Active;
                dbSubCategory.MainCategoryId = subCategory.MainCategoryId;
                dbSubCategory.ModifiedBy = User.Identity.GetUserId();
                dbSubCategory.ModifiedDate = DateTime.Now;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var model = new EditSubCategoryViewModel();
            model.Id = subCategory.Id;
            model.Name = subCategory.Name;
            model.Active = subCategory.Active;
            model.MainCategorys = db.MainCategorys.OrderBy(p => p.Name).Where(p => p.Active == true).ToList();
            model.MainCategory = await db.MainCategorys.SingleOrDefaultAsync(p => p.Id == subCategory.MainCategoryId);

            return View(model);
        }

        // GET: SubCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await db.SubCategorys.Include(p => p.MainCategory).FirstOrDefaultAsync(p => p.Id == id);
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
            subCategory.Active = false;
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
