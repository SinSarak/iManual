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
using iManual.Models.ViewModels.ArticalViewModels;
using iManual.Helper;
using iManual.Models.EnumBase;
using Microsoft.AspNet.Identity;
using iManual.Models.ViewModels.ArticleContentViewModels;

namespace iManual.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public async Task<ActionResult> Index()
        {
            var articles = db.Articles.Include(a => a.SubCategory);
            return View(await articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            var model = new CreateArticleViewModel();
            model.SubCategorys = db.SubCategorys.OrderBy(p => p.Name).Where(p => p.Active).ToList();
            return View(model);
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateArticleModel article)
        {
            if (ModelState.IsValid)
            {
                var model = new Article();
                model.Title = article.Title.TrimNullable();
                model.Description = article.Description.TrimNullable();
                model.SubCategoryId = article.SubCategoryId;
                model.Status = (int)ArticleStatus.Active;
                model.UserId = User.Identity.GetUserId();
                model.PublishStatus = (int)ArticlePublishStatus.Done;
                model.CreatedBy = User.Identity.GetUserId();
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                db.Articles.Add(model);
                await db.SaveChangesAsync();

                var contents = article.GetArticleContents();
                int i = 0;
                foreach(var content in contents)
                {
                    var articleContent = new ArticleContent();
                    articleContent.Title = content.Title;
                    articleContent.Description = content.Title;
                    articleContent.Active = true;
                    articleContent.ArticleId = model.Id;
                    articleContent.Order = i;
                    articleContent.FileName = content.FileName;
                    articleContent.FullPath = content.FullPath;
                    articleContent.UploadedDate = DateTime.Now;
                    i++;
                    db.ArticleContents.Add(articleContent);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ViewBag.SubCategoryId = new SelectList(db.SubCategorys, "Id", "Name", article.SubCategoryId);
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubCategoryId = new SelectList(db.SubCategorys, "Id", "Name", article.SubCategoryId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Status,PublishStatus,SubCategoryId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,UserId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubCategoryId = new SelectList(db.SubCategorys, "Id", "Name", article.SubCategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Article article = await db.Articles.FindAsync(id);
            db.Articles.Remove(article);
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
