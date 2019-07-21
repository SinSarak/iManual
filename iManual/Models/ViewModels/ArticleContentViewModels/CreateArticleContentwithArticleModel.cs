using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.ArticleContentViewModels
{
    public class CreateArticleContentwithArticleModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
    }
}