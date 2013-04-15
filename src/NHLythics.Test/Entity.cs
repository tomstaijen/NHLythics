using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHLythics.Extensions;

namespace NHLythics.Test
{
    public class Entity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
    }

    class EntityMap : ClassMap<Entity>
    {
        public EntityMap()
        {
            Table("Entity");
            Id(e => e.Id).GeneratedBy.Native();
            Map(e => e.Name, "Name").Length(40);
            this.Synonym("OtherDatabase.dbo.Entity");
        }
    }
}
