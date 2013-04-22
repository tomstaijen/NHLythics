using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;

namespace NHLythics.Model
{
    public class MappingModel
    {
        public MappingModel()
        {

            _entities = new Dictionary<string, Entity>();
        }

        private IDictionary<string, Entity> _entities;
        public IDictionary<string, Entity> Entities { get { return _entities;  } }

        public void Validate(Entity entity)
        {
        }

        public Entity GetEntityByName(string name)
        {
            if (!_entities.ContainsKey(name))
                return null;
            return _entities[name];
        }

        public void AddEntity(string name, Entity entity)
        {
            _entities.Add(name, entity);
        }
    }
}
