using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Subscription
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int SubscribedTo { get; set; }
    }
}
