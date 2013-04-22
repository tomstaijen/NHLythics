using System;
using System.Collections.Generic;
using System.Linq;
using NHLythics.Model;
using NHibernate.Cfg;

namespace NHLythics.Processors
{
    class EntityHarvester : ModelCheckerModuleBase
    {
        public Configuration Configuration { get; set; }

        public override void Run()
        {
            foreach (var group in Configuration.ClassMappings.GroupBy(cm => cm.Table))
            {
                var entity = new Entity(Model) { Name = group.Key.Name };
                entity.Classes.AddRange(group.ToList());
                Model.AddEntity(entity.Name, entity);
            }

            foreach (var group in Configuration.CollectionMappings.GroupBy(cm => cm.CollectionTable ?? cm.Table))
            {

                var entity = Model.GetEntityByName(group.Key.Name);
                if (entity == null)
                {
                    entity = new Entity(Model) { Name = group.Key.Name };
                    Model.AddEntity(entity.Name, entity);
                }
                entity.Collections.AddRange(group.ToList());
            }
        }
    }
}
