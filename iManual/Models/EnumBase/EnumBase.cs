using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.EnumBase
{
    
    public enum UserStatus
    {
        Deactive = 0,
        Active = 1 ,
        Suspended = 2
    }
        
    public enum ArticleStatus
    {
        Deactive = 0,
        Active = 1,
        Suspended  =2
    }
    public enum ArticlePublishStatus
    {
        Draft = 0,
        Done = 1
    }
    
}