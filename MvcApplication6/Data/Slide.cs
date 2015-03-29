using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orchard;

namespace Data
{
    public class Slide
    {
        public virtual int Id { get; set; }
      //  [StringLengthMax]
        public virtual string Text { get; set; }
        public virtual int SlideNo { get; set; }
        public virtual Presentation Presentation { get; set; }
    }
}
