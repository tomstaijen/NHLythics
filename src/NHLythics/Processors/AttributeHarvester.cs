using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLythics.Model;
using Attribute = NHLythics.Model.Attribute;

namespace NHLythics
{
    public class AttributeHarvester : IEntityProcessor
    {
        private MappingModel _model;
        public AttributeHarvester(MappingModel model)
        {
            _model = model;
        }

        private IDictionary<Type, Entity> _typeMap;

        public IDictionary<Type, Entity> GetTypeMap()
        {
            if (_typeMap == null)
            {
                _typeMap = new Dictionary<Type, Entity>();
                foreach (var e in _model.Entities.Values)
                {
                    foreach (var cm in e.Classes)
                    {
                        _typeMap.Add(cm.MappedClass, e);
                    }
                }

            }
            return _typeMap;
        }



        public IEnumerable<Problem> Process(Entity entity)
        {
            foreach (var cm in entity.Classes)
            {
                foreach (var property in cm.PropertyIterator)
                {

                    var a = new Attribute {Name = property.Name, Property = property, Parent = entity };

                    if (a.Property.IsEntityRelation)
                    {
                        a.ReferencedEntity = GetTypeMap()[a.Property.Type.ReturnedClass];
                        if (a.ReferencedEntity == null)
                        {
                            yield return new Problem {Location = a, Solution = "ADD MAPPING", Description= "Referenced class not mapped"};
                        }
                    }
                    entity.Attributes.Add(a);
                }
            }
            //return Enumerable.Empty<Problem>();
        }
    }
}
