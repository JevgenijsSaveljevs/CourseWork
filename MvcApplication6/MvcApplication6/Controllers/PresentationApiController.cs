using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bussines;
using AutoMapper;
using MvcApplication6.Models;


namespace MvcApplication6.Controllers
{
    public class PresentationApiController : ApiController
    {
        [HttpGet]
        public IEnumerable<PresentationModel> GetPrez()
        {
            using (var ent = new Entities<Presentation>())
            {
               var result = ent.GetAllPrezs().Select( x => Mapper.Map<Presentation, PresentationModel>(x)).ToList();
               return result;
            }
        }
    }
}
