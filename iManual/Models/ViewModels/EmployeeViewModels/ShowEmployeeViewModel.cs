using iManual.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.EmployeeViewModels
{
    public class ShowEmployeeViewModel
    {
        public List<MainCategory> MainCategorys { get; set; }
        public List<SubCategory> SubCategorys { get; set; }
        public List<Article> Articles { get; set; }
    }
}