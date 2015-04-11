using Data;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Data;

namespace Bussines
{
    public class Repository<T> : IDisposable where T : class
    {
        public ISession Session { get; private set; }

        NHibernateHelper helper = new NHibernateHelper("Data Source=.;Initial Catalog=Nhibernate;Integrated Security=True");

        public Repository()
        {
            Session = helper.SessionFactory.OpenSession();
            Session.FlushMode = FlushMode.Auto;

             Session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Flush()
        {
            Session.Flush();
        }


        #region IRepository<T> Members

        public bool Add(T entity)
        {
            //Session.SaveOrUpdate(entity);
           Session.Save(entity);
            return true;
        }

        public T ById(int id)
        {
            return Session.Get<T>(id);
        }

        public bool Add(System.Collections.Generic.IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Session.Save(item);
            }
            return true;
        }

        //public T GetAll()
        //{
        //    return Session.Get<T>();
        //}

        public bool Update(T entity)
        {
            Session.Update(entity);
            return true;
        }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }

        public IQueryable<Customer> Test
        {
            get { return Session.Query<Customer>(); }//Repository.Query<ProductType>(); }
        }

        public bool Delete(T entity)
        {
            Session.Delete(entity);
            return true;
        }

        public bool Delete(System.Collections.Generic.IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Session.Delete(entity);
            }
            return true;
        }

        #endregion

        #region IIntKeyedRepository<T> Members

        public T FindBy(int id)
        {
            return Session.Get<T>(id);
        }

        #endregion

        #region IReadOnlyRepository<T> Members

        public IQueryable<T> All()
        {
            throw new NotImplementedException("I don't know what is session.Linq<T>");
        }

        public T FindBy(System.Linq.Expressions.Expression<System.Func<T, bool>> expression)
        {
            return FilterBy(expression).Single();
        }

        public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<System.Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        #endregion

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public void Dispose()
        {

            Session.Transaction.Commit();
            Session.Close();
        }

        public Presentation CreatePresentation(Presentation ppt)
        {
            this.Session.Save(ppt);
            using (var Db = new Entities<Presentation>())
            {
                var slides = ppt.Pages;
                ppt.Pages = null;
                Db.Add(ppt);

                var DBPRez = Db.GetAll().Where(x => x.Created == ppt.Created && x.Owner == ppt.Owner).FirstOrDefault();

                //    foreach (var item in slides)
                //    {
                //        item.Presentation = DBPRez;
                //        CreateSlide(item);
                //    }
                //}
            }

            using (var Db = new Repository<Presentation>())
            {
                var slides = ppt.Pages;
                ppt.Pages = null;
                Db.Add(ppt);

                var DBPRez = Db.GetAll().Where(x => x.Created == ppt.Created && x.Owner == ppt.Owner).FirstOrDefault();

                //foreach (var item in slides)
                //{
                //    item.Presentation = DBPRez;
                //    CreateSlide(item);
                //}
            }
            return (ppt as Presentation);
        }
    }
}
