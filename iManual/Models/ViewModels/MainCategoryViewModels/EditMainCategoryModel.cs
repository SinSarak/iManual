using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.MainCategoryViewModels
{
    public class EditMainCategoryModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}