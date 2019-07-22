using iManual.Models;
using iManual.Models.ViewModels.EmployeeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iManual.Models.EnumBase;
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
    }
}