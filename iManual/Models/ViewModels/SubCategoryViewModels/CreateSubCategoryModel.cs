using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.SubCategoryViewModels
{
    public class CreateSubCategoryModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int MainCategoryId { get; set; }
    }
}