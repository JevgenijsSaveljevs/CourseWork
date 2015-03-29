using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class NHibernateHelper
    {
        private readonly string _connectionString;
        private ISessionFactory _sessionFactory;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }

        }

        public NHibernateHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Data Source=.;Initial Catalog=Nhibernate;Integrated Security=True"))
                .Mappings(m =>
                        m.FluentMappings
                    .AddFromAssemblyOf<Bussines.Mappings.BigMap>())
                    .BuildSessionFactory();
        }
    }
}
