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
            _problems = new List<Problem>();
            _entities = new Dictionary<string, Entity>();
        }

        private List<Problem> _problems;
        public List<Problem> Problems { get { return _problems;  } }


        private IDictionary<string, Entity> _entities;
        public IDictionary<string, Entity> Entities { get { return _entities;  } }

        public static MappingModel Build(Action<ModelBuilder> builder)
        {
            var model = new MappingModel();
            var modelBuilder = new ModelBuilder(model);

            builder(modelBuilder);
            
            return model;
        }


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

        /// <summary>
        /// Applies the processor and stores all problems.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public MappingModel Apply(IEntityProcessor processor)
        {
            Entities.Values.Each(e => Problems.AddRange(processor.Process(e)));
            // for method chaing
            return this;
        }
    }
}
