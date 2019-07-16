using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.Domains
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int MainCategoryId { get; set; }

        public MainCategory MainCategory { get; set; }
    }
}