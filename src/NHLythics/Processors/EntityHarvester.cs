using System;
using System.Collections.Generic;
using System.Linq;
using NHLythics.Model;
using NHibernate.Cfg;

namespace NHLythics.Processors
{
    class EntityHarvester : IModelBuilderExtension
    {
        public Configuration Configuration { get; set; }

        public IEnumerable<Problem> Build(MappingModel model)
        {

            foreach (var group in Configuration.ClassMappings.GroupBy(cm => cm.Table))
            {
                var entity = new Entity(model) { Name = group.Key.Name };
                entity.Classes.AddRange(group.ToList());
                model.AddEntity(entity.Name, entity);
            }

            foreach (var group in Configuration.CollectionMappings.GroupBy(cm => cm.CollectionTable ?? cm.Table))
            {

                var entity = model.GetEntityByName(group.Key.Name);
                if (entity == null)
                {
                    entity = new Entity(model) { Name = group.Key.Name };
                    model.AddEntity(entity.Name, entity);
                }
                entity.Collections.AddRange(group.ToList());
            }

            return Enumerable.Empty<Problem>();
        }
    }
}
