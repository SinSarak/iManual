using iManual.Models;
using iManual.Models.ViewModels.EmployeeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iManual.Models.EnumBase;
using System.Net;
using System.Data.Entity;
namespace iManual.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Employee
        public ActionResult Index()
        {
            var model = new ShowEmployeeViewModel();
            model.MainCategorys = db.MainCategorys.OrderBy(p => p.Name).Where(p => p.Active == true).ToList();
            model.SubCategorys = db.SubCategorys.Include("MainCategory").OrderBy(p => p.Name).Where(p => p.Active == true && p.MainCategory.Active == true).ToList();
            model.Articles = db.Articles.Include("SubCategory").OrderBy(p => p.Title)
                            .Where(p=>p.Status == (int)ArticleStatus.Active && p.PublishStatus == (int)ArticlePublishStatus.Done && p.SubCategory.Active == true).ToList();
            return View(model);
        }

        public ActionResult testing()
        {
            return View();
        }

        public ActionResult ArticleView(int? id)
        {
            var model = new ShowArticleViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else
            {
                if (db.Articles.SingleOrDefault(p=>p.Id == id) == null)
                {
                    return HttpNotFound();
                }
                model.Article = db.Articles.Include("SubCategory").Include(p=>p.User.UserInformation).SingleOrDefault(p=>p.Id == id);
                model.ArticleContent = db.ArticleContents.OrderBy(p => p.Order).Where(p=>p.ArticleId == model.Article.Id).ToList();
                
            }
            return View(model);
        }
        public FileResult ViewPDF(int? id)
        {
            var pdf = "";
            if (id != null)
            {
                pdf = db.ArticleContents.SingleOrDefault(p=>p.Id == id).FullPath;
            }
            return File(pdf, "application/pdf");
        }
    }
}