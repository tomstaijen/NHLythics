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
        public virtual C MyC { get; set; }
    }

    public class B
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<A> As { get; set; }
    }

    public class C
    {
        public virtual long Id { get; set; }
        public ICollection<A> As { get; set; }
    }

    class AMap : ClassMap<A>
    {
        public AMap()
        {
            Table("A");
            Id(e => e.Id).GeneratedBy.Native();
            Map(e => e.Name, "ilnombre").Length(40);
            References(e => e.MyB, "TheB").Not.Nullable();
            References(e => e.MyC, "SomeC").Not.Nullable();
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

    class CMap : ClassMap<C>
    {
        public CMap()
        {
            Table("C");
            Id(e => e.Id).GeneratedBy.Native();
            HasManyToMany(c => c.As).ParentKeyColumn("TheC").ChildKeyColumn("TheA").Table("C_A_X");
        }
    }
}
