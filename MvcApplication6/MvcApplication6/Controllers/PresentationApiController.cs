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
                var result = ent.GetAllPrezs().Select(x => Mapper.Map<Presentation, PresentationModel>(x)).ToList();
                return result;
            }
        }

        [HttpGet]
        public string ActivatePpt(long id)
        {
            try
            {
                using (var ent = new Entities<Presentation>())
                {
                    ent.Activate(id);
                    return id.ToString();
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }

        [HttpGet]
        public IEnumerable<UserDB> GetUsers()
        {
            try
            {
                using (var ent = new Entities<UserDB>())
                {
                    return ent.GetUsers();
                    //return id.ToString();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        public string deactivatePpt(long id)
        {
            try
            {
                using (var ent = new Entities<Presentation>())
                {
                    ent.Dectivate(id);
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }

        [HttpGet]
        public string GetBRSlide(int id)
        {
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    var result = ent.GetBR(id);
                    return result.Text;
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }

        [HttpGet]
        public string IncBRSlide(int id)
        {
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    ent.Inc(id);
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }


        [HttpGet]
        public string DecBRSlide(int id)
        {
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    ent.Dec(id);
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }

        [HttpGet]
        public string CurBRSlide(int id)
        {
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    var rez = ent.CurrentSlide(id);
                    return rez.ToString();
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }

        [HttpGet]
        public string CreateBroadcast(int PrezId)
        {
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    ent.CreateBroadCast(PrezId);
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }




    }
}
