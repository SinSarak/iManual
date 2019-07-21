using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iManual.Helper;
using iManual.Models.ViewModels.ArticleContentViewModels;

namespace iManual.Controllers
{
    public class ArticleContentController : Controller
    {
        // GET: ArticleContent
        public ActionResult UploadContent()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                long maxSize = 20 * (1024 * 1024);
                //string ImagePath = Request.Form["ImagePath"];
                string path = "";
                //List<UploadAttachmentTicketViewModel> model = new List<UploadAttachmentTicketViewModel>();

                for (int i = 0; i < files.Count; i++)
                {
                    //Checking file is available to save.  

                    HttpPostedFileBase file = files[i];
                    if (file.ContentLength > maxSize)
                    {
                        return Json(new { success = false, responseText = "Maximun upload file size: 10MB." }, JsonRequestBehavior.AllowGet);
                    }
                    if (file.FileName == "")
                    {
                        return Json(new { success = false, responseText = "Please Choose File!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (!HelperFunction.CheckExtPDF(file.FileName))
                    {
                        return Json(new { success = false, responseText = "File must be Image or PDF type." }, JsonRequestBehavior.AllowGet);
                    }
                    string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                    string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                    string datetime = DateTime.Now.ToString("yyyymmddhhmmss");

                    string filename = name + "_" + datetime + ext;

                    var model = new ReturnArticleContentModel();
                    model.filename = filename;
                    model.path = "/Uploads/ArticleContents/" + filename;
                    model.tempid = datetime;

                    var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/ArticleContents/") + filename);
                    //Save file to server folder  
                    file.SaveAs(ServerSavePath);

                    return Json(new { success = true, response = model }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, responseText = "Server Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "Server Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}