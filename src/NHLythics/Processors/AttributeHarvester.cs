using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLythics.Model;
using Attribute = NHLythics.Model.Attribute;

namespace NHLythics
{
    public class AttributeHarvester : EntityCheckerBase
    {
        private IDictionary<Type, Entity> _typeMap;

        public IDictionary<Type, Entity> GetTypeMap()
        {
            if (_typeMap == null)
            {
                _typeMap = new Dictionary<Type, Entity>();
                foreach (var e in ModelChecker.Model.Entities.Values)
                {
                    foreach (var cm in e.Classes)
                    {
                        _typeMap.Add(cm.MappedClass, e);
                    }
                }

            }
            return _typeMap;
        }

        public override void Check(Entity entity)
        {
            foreach (var cm in entity.Classes)
            {
                foreach (var property in cm.PropertyIterator)
                {

                    var a = new ClassAttribute {Name = property.Name, Property = property, Parent = entity};

                    if (a.Property.IsEntityRelation)
                    {
                        a.ReferencedEntity = GetTypeMap()[a.Property.Type.ReturnedClass];
                        if (a.ReferencedEntity == null)
                        {
                            RegisterProblem(new Problem
                                    {
                                        Location = a,
                                        Solution = "ADD MAPPING",
                                        Description = "Referenced class not mapped"
                                    });
                        }
                    }
                    entity.Attributes.Add(a);
                }
            }

            foreach (var cm in entity.Collections)
            {
                foreach (var column in cm.Key.ColumnIterator )
                {
                    entity.Attributes.Add(new Attribute
                        {
                            Name = column.Text, 
                            ReferencedEntity = Model.GetEntityByName(cm.Table.Name)
                        });
                }

                RegisterProblem(new Problem {Description = "Help!"});
            }
        }
    }
}
