using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Presentation
    {
        public virtual int Id { get; set; }
        public virtual string Name {get; set;}
        public virtual int Owner { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual bool isActive { get; set; }
        public virtual IEnumerable<Slide> Pages { get; set; }
    }
}
