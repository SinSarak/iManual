using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.Domains
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int PublishStatus { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}