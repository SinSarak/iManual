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
using iManual.Models.ViewModels.MainClaimViewModels;
using iManual.Helper;

namespace iManual.Controllers
{
    public class MainClaimController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MainClaims
        public async Task<ActionResult> Index()
        {
            var mainClaims = db.MainClaims.Include(m => m.SubClaim);
            return View(await mainClaims.ToListAsync());
        }

        // GET: MainClaims/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainClaim mainClaim = await db.MainClaims.FindAsync(id);
            if (mainClaim == null)
            {
                return HttpNotFound();
            }
            return View(mainClaim);
        }

        // GET: MainClaims/Create
        public ActionResult Create()
        {
            var model = new CreateMainClaimViewModel();
            model.MainClaims = db.MainClaims.OrderBy(p => p.Name).Where(p=>p.SubClaimId != 0 && p.Active == true).ToList();
            return View(model);
        }

        // POST: MainClaims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateMainClaimModel mainClaim)
        {
            var model = new MainClaim();
            model.Name = mainClaim.Name.TrimNullable().ToUpperNullable();

            if (mainClaim.SubClaimId != 0)
            {
                model.SubClaimId = mainClaim.SubClaimId;
            }
            model.Active = true;

            db.MainClaims.Add(model);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: MainClaims/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainClaim mainClaim = await db.MainClaims.FindAsync(id);
            if (mainClaim == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubClaimId = new SelectList(db.MainClaims, "Id", "Name", mainClaim.SubClaimId);
            return View(mainClaim);
        }

        // POST: MainClaims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Active,SubClaimId")] MainClaim mainClaim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mainClaim).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubClaimId = new SelectList(db.MainClaims, "Id", "Name", mainClaim.SubClaimId);
            return View(mainClaim);
        }

        // GET: MainClaims/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainClaim mainClaim = await db.MainClaims.FindAsync(id);
            if (mainClaim == null)
            {
                return HttpNotFound();
            }
            return View(mainClaim);
        }

        // POST: MainClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MainClaim mainClaim = await db.MainClaims.FindAsync(id);
            db.MainClaims.Remove(mainClaim);
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
