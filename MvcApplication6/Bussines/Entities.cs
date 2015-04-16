using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class Entities<T> : Repository<T> where T : class
    {
        public Entities()
            : base()
        {

        }

        public IQueryable<Customer> GetCustomers()
        {
            var cust = new Entities<Customer>();
            return cust.GetAll();
        }

        public IQueryable<Slide> GetSlides()
        {
            var slides = new Entities<Slide>();
            return slides.GetAll();
        }

        public IQueryable<Presentation> GetPresentations()
        {
            var ppt = new Entities<Presentation>();
            return ppt.GetAll();
        }

        public void CreateSlide(Slide slide)
        {
            var db = new Entities<Slide>();
            db.Add(slide);
        }

        public Broadcast GetBR(int pptId)
        {
            var db = new Entities<Broadcast>();
            //return db.All()
            return db.GetAll().Where(x => x.PptId == pptId).FirstOrDefault();
        }

        public IQueryable<Presentation> GetAllPrezs()
        {
            var db = new Entities<Presentation>();
            return db.GetAll();
        }

        public void Activate(long id)
        {
            var db = new Entities<Presentation>();
            var activate = db.GetAll().Where(x => x.Id == id).FirstOrDefault();
            activate.isActive = true;
            db.Session.Transaction.Commit();

        }


        public IEnumerable<UserDB> GetUsers()
        {
            var db = new Entities<UserDB>();
            return db.GetAll().ToList();     
        }


        public IEnumerable<UserDB> GetPagedUsers(long id,int portionSize, int Page)
        {
            var db = new Entities<UserDB>();
            var ttl = db.GetAll().Count();

            var skip = (Page - 1) * portionSize;
            
            return db.GetAll()
                .Skip(skip)
                .Take(portionSize)
                .ToList();
        }

        public void Dectivate(long id)
        {
            var db = new Entities<Presentation>();
            var activate = db.GetAll().Where(x => x.Id == id).FirstOrDefault();

            activate.isActive = false;
            db.Session.Transaction.Commit();
        }


        public void DeletePpt(long id)
        {
            var db = new Entities<Presentation>();
            var activate = db.GetAll().Where(x => x.Id == id).FirstOrDefault();

            foreach (var item in activate.Pages)
            {
                using (var ent = new Entities<Slide>())
                {
                    ent.Delete(item);
                }
            }
            db.Session.Flush();
            activate.Pages = null;
            using(var ent = new Entities<Presentation>())
                db.Delete(activate);

            db.Session.Transaction.Commit();
        }

        public void CreateSub(int userId, int subscribeTo)
        {
            var db = new Entities<Subscription>();
            
            if(db.GetAll().Where(x => x.UserId == userId && x.SubscribedTo == subscribeTo).Count() == 0)
                db.Session.SaveOrUpdate(new Subscription
                {
                 UserId = userId,
                 SubscribedTo = subscribeTo
                }); 
            db.Session.Transaction.Commit();
        }

        public void DeleteSub(int userId, int subscribeTo)
        {
            var db = new Entities<Subscription>();
            var toDel = db.GetAll().Where(x => x.UserId == userId && x.SubscribedTo == subscribeTo).FirstOrDefault();
            if (toDel != null)
            {
                db.Delete(toDel);
                db.Session.Transaction.Commit();
            }
        }

        public IEnumerable<Subscription> GetSubForUser(int userId)
        {
            var db = new Entities<Subscription>();
            return db.GetAll().Where(x => x.UserId == userId).ToList();
        }

        public void Inc(long id)
        {
            int slideNo = 0;
            string txt = String.Empty;
            using (var db = new Entities<Broadcast>())
            {
                var br = db.GetAll().Where(x => x.PptId == id).FirstOrDefault();
                slideNo = br.SlideId;
                slideNo = slideNo + 1;
                br.SlideId = slideNo;
                db.Session.Flush();
            }


            using (var db2 = new Entities<Slide>())
            {
                var result = db2.GetAll().Where(x => x.Presentation.Id == id && x.SlideNo == slideNo).FirstOrDefault();
                txt = result.Text;
            }
            using (var db3 = new Entities<Broadcast>())
            {
                //  db.Session.Transaction.Begin();
                var updateBr = db3.GetAll().Where(x => x.PptId == id).FirstOrDefault();
                updateBr.Text = txt;
                db3.Session.Flush();
            }

        }

        public int TotalSlideCount(int pptId)
        {
            var db = new Entities<Presentation>();
            return db.GetAllPrezs().Where(x => x.Id == pptId).FirstOrDefault().Pages.Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pptId">Presentation Id in Broadcast</param>
        /// <returns></returns>
        public int CurrentSlide(int pptId)
        {
            var db = new Entities<Broadcast>();
            return db.GetAll().Where(x => x.PptId == pptId).FirstOrDefault().SlideId;
        }


        public void Dec(long id)
        {
            int slideNo = 0;
            string txt = String.Empty;
            using (var db = new Entities<Broadcast>())
            {
                var br = db.GetAll().Where(x => x.PptId == id).FirstOrDefault();
                slideNo = br.SlideId;
                slideNo = slideNo - 1;
                br.SlideId = slideNo;
                db.Session.Flush();
            }


            using (var db2 = new Entities<Slide>())
            {
                var result = db2.GetAll().Where(x => x.Presentation.Id == id && x.SlideNo == slideNo).FirstOrDefault();
                txt = result.Text;
            }
            using (var db3 = new Entities<Broadcast>())
            {
                //  db.Session.Transaction.Begin();
                var updateBr = db3.GetAll().Where(x => x.PptId == id).FirstOrDefault();
                updateBr.Text = txt;
                db3.Session.Flush();
            }
        }

        public Presentation CreatePresentation(Presentation ppt)
        {
            var slides = ppt.Pages;
            using (var Db = new Entities<Presentation>())
            {

                ppt.Pages = null;
                Db.Add(ppt);

                var DBPRez = Db.GetAll().Where(x => x.Created == ppt.Created && x.Owner == ppt.Owner).FirstOrDefault();
                Db.Flush();


            }

            foreach (var item in slides)
            {
                using (var db = new Entities<Slide>())
                {
                    db.Add(item);
                }// item.Presentation = DBPRez;
                //var ll = item.Text.Length;

                //  CreateSlide(item);
            }

            //using (var Db = new Repository<Presentation>())
            //{
            //    var slides = ppt.Pages;
            //    ppt.Pages = null;
            //    Db.Add(ppt);

            //    var DBPRez = Db.GetAll().Where(x => x.Created == ppt.Created && x.Owner == ppt.Owner).FirstOrDefault();

            //    //foreach (var item in slides)
            //    //{
            //    //    item.Presentation = DBPRez;
            //    //    CreateSlide(item);
            //    //}
            //}
            return (ppt as Presentation);
        }

        public void CreatePresentationBasic(Presentation ppt)
        {
            var pp = new Entities<Presentation>();
            pp.Add(ppt);
        }

        public Broadcast CreateBroadCast(int PrezId)
        {
            Presentation ppt = new Presentation();
            using (var db = new Entities<Presentation>())
            {
                ppt = db.GetAllPrezs().Where(x => x.Id == PrezId).FirstOrDefault();
            }

            Broadcast br = new Broadcast
            {
                PptId = ppt.Id,
                SlideId = 0,
                Text = (ppt.Pages.Count() > 0) ? ppt.Pages.Where(x => x.SlideNo == 0).FirstOrDefault().Text : String.Empty
            };

            using (var db = new Entities<Broadcast>())
            {
                db.Add(br);
            }

            return br;
        }

    }
}
