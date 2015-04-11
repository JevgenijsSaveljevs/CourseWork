using Data;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Mappings
{
    public class BigMap
    {
        public partial class CustomerMap : ClassMap<Customer>
        {
            public CustomerMap()
            {
                Id(x => x.Id).Not.Nullable();
                Map(x => x.Name);
            }
        }


        public class PresentationMap : ClassMap<Presentation>
        {
            public PresentationMap()
            {
                Table("Presentation1");
                Id(x => x.Id).GeneratedBy.Increment();
                //  Id(x => x.Id).GeneratedBy.Assigned();
                Map(x => x.Created);
                Map(x => x.Owner).Not.Nullable();
                Map(x => x.isActive);
                HasMany(x => x.Pages).KeyColumn("PptId").Not.LazyLoad().Cascade.None();
                //References(x => x.Pages).LazyLoad(Laziness.False).Cascade.DeleteOrphans();

            }
        }

        public class StringTableHiLoGenerator : TableHiLoGenerator
        {
            public override object Generate(ISessionImplementor session, object obj)
            {
                return base.Generate(session, obj).ToString();
            }

            public override void Configure(IType type, System.Collections.Generic.IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
            {
                base.Configure(NHibernateUtil.Int32, parms, dialect);
            }
        }


        public class SlideMap : ClassMap<Slide>
        {
            public SlideMap()
            {
                Table("PptSlide1");
                Id(x => x.Id).GeneratedBy.Increment();
                //  Id(x => x.Id).GeneratedBy.Assigned();//.GeneratedBy.Identity();
                // HasOne(x => x.Presentation).Constrained().ForeignKey(;
                References(x => x.Presentation, "PptId").LazyLoad(Laziness.False).Not.Nullable();
                //Map(x => x.Presentation.Id, "PptId").Not.Nullable();
                Map(x => x.SlideNo).Not.Nullable();
                Map(x => x.Text).Not.Nullable().CustomSqlType("nvarchar(max)").Length(200000);
            }
        }

        public class BroadCastMap : ClassMap<Broadcast>
        {
            public BroadCastMap()
            {
                Id(x => x.Id).GeneratedBy.Increment();
                Map(x => x.PptId);
                Map(x => x.SlideId);
                Map(x => x.Text).CustomSqlType("nvarchar(max)").Length(200000);
                //  Map(x => x.Slide.Presentation.Id).Column("PptId");
            }
        }

    }
}
