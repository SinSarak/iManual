using iManual.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.EmployeeViewModels
{
    public class ShowArticleViewModel
    {
        public Article Article { get; set; }
        public List<ArticleContent> ArticleContent { get; set; }
    }
}