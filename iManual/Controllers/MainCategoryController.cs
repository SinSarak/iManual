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
using iManual.Models.ViewModels.MainCategoryViewModels;
using iManual.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace iManual.Controllers
{
    public class MainCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public MainCategoryController() { }
        public MainCategoryController(ApplicationUserManager userManager)
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

        [Authorize(Roles = "Admin,MAINCATEGORY_ALL")]
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

        [Authorize(Roles = "Admin,MAINCATEGORY_ALL,MAINCATEGORY_CREATE")]
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
            var model = new EditMainCategoryViewModel();
            if (mainCategory == null)
            {
                return HttpNotFound();
            }else
            {
                model.Id = mainCategory.Id;
                model.Name = mainCategory.Name;
                model.Active = mainCategory.Active;
            }
            return View(model);
        }

        // POST: MainCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditMainCategoryModel mainCategory)
        {
            if (ModelState.IsValid)
            {
                var dbMainCategory = await db.MainCategorys.SingleOrDefaultAsync(p=>p.Id == mainCategory.Id);
                if(dbMainCategory != null)
                {
                    dbMainCategory.Name = mainCategory.Name.TrimNullable();
                    dbMainCategory.Active = mainCategory.Active;
                    dbMainCategory.ModifiedBy = User.Identity.GetUserId();
                    dbMainCategory.ModifiedDate = DateTime.Now;
                }

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
            var model = new  DeleleMainCategoryViewModel();
            if (mainCategory == null)
            {
                return HttpNotFound();
            }else
            {
                model.Id = mainCategory.Id;
                model.Name = mainCategory.Name;
                model.Active = model.Active;
                model.CreatedDate = model.CreatedDate;
                model.ModifiedDate = model.ModifiedDate;

                var dbUserCreater =await UserManager.FindByIdAsync(mainCategory.CreatedBy);
                model.CreatedBy = dbUserCreater != null ? dbUserCreater.UserInformation.Accountname : "" ;
                var dbUserModifier = await UserManager.FindByIdAsync(mainCategory.ModifiedBy);
                model.CreatedBy = dbUserModifier != null ? dbUserModifier.UserInformation.Accountname : "" ;

                
            }
            return View(model);
        }

        // POST: MainCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MainCategory mainCategory = await db.MainCategorys.FindAsync(id);
            mainCategory.Active = false;
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
