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
using System.Web;


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
                var result = ent.GetAllPrezs().Where(x => x.Owner == WebMatrix.WebData.WebSecurity.CurrentUserId).Select(x => Mapper.Map<Presentation, PresentationModel>(x)).ToList();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Presentation Id</param>
        /// <returns></returns>
        [HttpGet]
        public int GetBroadCastId(int id)
        {
            using (var ent = new Entities<Broadcast>())
            {
                var result = ent.GetAll().Where(x => x.PptId == id).FirstOrDefault().Id;
                return result;
            }
        }

        [HttpGet]
        public string GetOwner(int id)
        {
            int brId = 0;
            int ownerId =0;
            int pptId = 0;
            using (var ent = new Entities<Broadcast>())
            {
                brId = ent.GetAll().Where(x => x.PptId == id).FirstOrDefault().PptId;
            }
            if (brId != 0)
            {
                using (var ent = new Entities<Presentation>())
                {
                    var ow = ent.GetAll().Where(x => x.Id == brId).FirstOrDefault();
                    ownerId = ow.Owner;
                }
                using (var ent = new Entities<Presentation>())
                {
                    return ent.GetUsers().Where(x => x.Id == ownerId).FirstOrDefault().UserName;
                }
            }
            else return "";
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
            int brId = 0;
            try
            {
                using (var ent = new Entities<Broadcast>())
                {
                    ent.CreateBroadCast(msg.PrezId);
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }

            using (var ent = new Entities<UserDB>())
            {
                var usrs = ent.GetSubForUser(WebMatrix.WebData.WebSecurity.CurrentUserId);
                foreach (var item in usrs)
                {
                    sendMail(item.SubscribedTo, User.Identity.Name, msg);
                }
              //  return "done";
            }

            return "done";
           

        }

        [HttpGet]
        public string DeleteBroadcast(int id)
        {

            Broadcast toDel;
            using (var ent = new Entities<Broadcast>())
            {
                toDel = ent.GetAll().Where(x => x.PptId == id).FirstOrDefault();
                ent.Delete(toDel);
            }
            try
            {
                using (var ent = new Entities<Presentation>())
                {
                    ent.Dectivate(toDel.PptId); 
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }


        [HttpGet]
        public string DeletePpt(int id)
        {
            Broadcast toDel;
            using (var ent = new Entities<Broadcast>())
            {
                toDel = ent.GetAll().Where(x => x.PptId == id).FirstOrDefault();
                if(toDel != null)
                    ent.Delete(toDel);
            }
            try
            {
                using (var ent = new Entities<Presentation>())
                {
                    ent.DeletePpt(id);
                    return "done";
                }
            }
            catch (Exception ex) { return "err = " + ex.Message; }
        }


      
        private void sendMail(int UserId, string sender, Message msg)
        {
            //Request.RequestUri.PathAndQuery.
            
         
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
                    var r1 = Request;
                    var r2 = HttpContext.Current;
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        
                        Subject = "User " + sender + " invited you to broadcast",
                        Body = String.Join(Environment.NewLine, HttpUtility.UrlDecode(msg.txt), "Your Link to broadcast: " + Request.Headers.Host +@"/"+ @"Presentation/Broadcast?id="+msg.PrezId)
                    })
                    {
                        smtp.Send(message);
                    }
                }
            }
   
        }




    }
}
