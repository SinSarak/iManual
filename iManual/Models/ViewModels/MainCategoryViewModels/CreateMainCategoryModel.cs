﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.MainCategoryViewModels
{
    public class CreateMainCategoryModel
    {
        [Required]
        public string Name { get; set; }
    }
}