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
        public Entities() : base()
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

        public IEnumerable<Presentation> GetAllPrezs()
        {
            var db = new Entities<Presentation>();
            return db.GetAll().ToList();
        }

        public Presentation CreatePresentation(Presentation ppt)
        {
            using (var Db = new Entities<Presentation>())
            {
                var slides = ppt.Pages;
                ppt.Pages = null;
                Db.Add(ppt);

                var DBPRez = Db.GetAll().Where(x => x.Created == ppt.Created && x.Owner == ppt.Owner).FirstOrDefault();
                Db.Flush();

                foreach (var item in slides)
                {
                    using(var db = new Entities<Slide>())
                    {
                        db.Add(item);
                    }// item.Presentation = DBPRez;
                    //var ll = item.Text.Length;
                   
                  //  CreateSlide(item);
                }
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

    }
}
