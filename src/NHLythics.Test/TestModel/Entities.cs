using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHLythics.Extensions;

namespace NHLythics.Test.TestModel
{
    public class A
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }

        public virtual B MyB { get; set; }
    }

    public class B
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<A> As { get; set; }
    }

    class AMap : ClassMap<A>
    {
        public AMap()
        {
            Table("A");
            Id(e => e.Id).GeneratedBy.Native();
            Map(e => e.Name, "Name").Length(40);
            References(e => e.MyB, "TheB").Not.Nullable();
            this.Synonym("NCyclopedie_new.dbo.ART");
        }
    }

    class BMap : ClassMap<B>
    {
        public BMap()
        {
            Table("B");
            //DiscriminateSubClassesOnColumn.("class");
            Id(e => e.Id).GeneratedBy.Native();
            HasMany(e => e.As).KeyColumn("TheB");
        }
    }
}
