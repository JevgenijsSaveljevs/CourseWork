using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Broadcast
    {
        public virtual int Id { get; set; }
        public virtual int SlideId { get; set; }
        public virtual int PptId { get; set; }
        public virtual string Text { get; set; }
    }
}
