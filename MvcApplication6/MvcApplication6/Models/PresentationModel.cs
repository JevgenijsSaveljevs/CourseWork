using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication6.Models
{
    public class PresentationModel
    {
        public int Id { get; set; }
        public int Owner { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<SlideModel> Pages { get; set; }
    }
}