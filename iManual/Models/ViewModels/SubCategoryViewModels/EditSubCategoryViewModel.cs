using iManual.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.SubCategoryViewModels
{
    public class EditSubCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<MainCategory> MainCategorys { get; set; }

        public MainCategory MainCategory { get; set; }
    }
}