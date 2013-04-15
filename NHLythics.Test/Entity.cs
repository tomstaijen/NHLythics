using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace NHLythics.Test
{
    public class Entity
    {
        public string Name { get; set; }
    }

    class EntityMap : ClassMap<Entity>
    {
        public EntityMap()
        {
            Table("Entity");
            Map(e => e.Name, "Name").Length(40);
        }
    }
}
