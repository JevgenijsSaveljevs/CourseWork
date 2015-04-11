using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication6.Models
{
    public class SlideModel
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual int SlideNo { get; set; }
        public virtual int PresentationId { get; set; }
    }
}