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
using MvcApplication6.Filters;
using System.Net.Mail;
using System.Web.Security;


namespace MvcApplication6.Controllers
{
   // [InitializeSimpleMembership]
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

        [HttpPost]
        public string UpdateSub(int[] Ids)
        {
            using (var ent = new Entities<Subscription>())
            {
                foreach (var item in Ids)
                {
                    ent.CreateSub(WebMatrix.WebData.WebSecurity.CurrentUserId, item);
                }

                return "updated";
            }
        }

        [HttpGet]
        public int[] SubListForUser()
        {
            using (var ent = new Entities<Subscription>())
            {
                var result =  ent.GetSubForUser(WebMatrix.WebData.WebSecurity.CurrentUserId).Select(x => x.SubscribedTo).ToArray();
                return result;
            }

        }

        [HttpPost]
        public string DeleteSub(int[] Ids)
        {
            using (var ent = new Entities<Subscription>())
            {
                foreach (var item in Ids)
                {
                    ent.DeleteSub(WebMatrix.WebData.WebSecurity.CurrentUserId, item);
                }

                return "updated";
            }
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

        [HttpPost]
        public string CreateBroadcast(Message msg)
        {
            using (var ent = new Entities<UserDB>())
            {
                var usrs = ent.GetSubForUser(WebMatrix.WebData.WebSecurity.CurrentUserId);
                foreach (var item in usrs)
                {
                    sendMail(item.SubscribedTo, User.Identity.Name, msg.txt);
                }
              //  return "done";
            }
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    ent.CreateBroadCast(msg.PrezId);
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }



        }

        private void sendMail(int UserId, string sender, string Body)
        {
            using (var ent = new Entities<UserDB>())
            {
                var usr = ent.GetUsers().Where(x => x.Id == UserId).FirstOrDefault();
                if (usr.Email != String.Empty)
                {
                    
                    var fromAddress = new MailAddress("banderge@gmail.com", "Jevgenijs Saveljevs");
                    var toAddress = new MailAddress(usr.Email, usr.UserName);
                    const string fromPassword = "!senger216";
                    const string subject = "Subject";
                    const string body = "Body";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = "User " + sender + " invited you to broadcast",
                        Body = Body
                    })
                    {
                        smtp.Send(message);
                    }
                }
            }
   
        }




    }
}
