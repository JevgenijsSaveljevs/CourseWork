using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bussines;

namespace MvcApplication6.Controllers
{
    public class PresentationApiController : ApiController
    {
        [HttpGet]
        public IEnumerable<Presentation> GetPrez()
        {
            using (var ent = new Entities<Presentation>())
            {
               var result = ent.GetAllPrezs();
               return result;
            }
        }
    }
}
