using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.Domains
{
    public class ArticleContent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string HrefName { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
        public int ArticleId { get; set; }
        public DateTime UploadedDate { get; set; }

        public Article Article { get; set; }

    }
}