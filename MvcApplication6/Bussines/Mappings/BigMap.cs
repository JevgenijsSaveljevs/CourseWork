using Data;
using FluentNHibernate.Mapping;
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
                
                Id(x => x.Id).Not.Nullable().GeneratedBy.Identity();
                Map(x => x.Created);
                Map(x => x.Owner).Not.Nullable();
                HasMany(x => x.Pages).KeyColumn("PptId").Not.LazyLoad().Cascade.DeleteOrphan();
                //References(x => x.Pages).LazyLoad(Laziness.False).Cascade.DeleteOrphans();

            }
        }


        public class SlideMap : ClassMap<Slide>
        {
            public SlideMap()
            {
                Table("PptSlide");

                Id(x => x.Id).Not.Nullable().GeneratedBy.Identity();
                References(x => x.Presentation, "PptId").LazyLoad(Laziness.False).Not.Nullable();
                //Map(x => x.Presentation.Id, "PptId").Not.Nullable();
                Map(x => x.SlideNo).Not.Nullable();
                Map(x => x.Text).Not.Nullable().CustomSqlType("nvarchar(max)").Length(200000);
            }
        }

    }
}
