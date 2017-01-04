using NgCookingKR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCookingKR.Models
{
    public class Communaute
    {
        public List<CreatorNote> CreatorNote { get; set; }
    }

    public class CreatorNote
    {
        public Creator Creator{get;set;}
        public double Note { get; set;}
      

    }
}