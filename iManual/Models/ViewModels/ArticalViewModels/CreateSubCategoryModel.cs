using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.ArticalViewModels
{
    public class CreateSubCategoryModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
    }
}