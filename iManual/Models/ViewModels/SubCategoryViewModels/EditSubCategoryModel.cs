using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.SubCategoryViewModels
{
    public class EditSubCategoryModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int MainCategoryId { get; set; }
    }
}