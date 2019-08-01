using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace iManual.Models
{
    public class MainClaim
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? SubClaimId { get; set; }

        [ForeignKey("SubClaimId")]
        public MainClaim SubClaim { get; set; }
    }
}