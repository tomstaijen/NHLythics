using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLythics.Model;
using NHibernate.Mapping;
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
                // TODO: put this into a union with the propertyiterator
                if (cm.HasIdentifierProperty)
                {
                    var column = cm.IdentifierProperty.ColumnIterator.First() as Column;
                    entity.AddAttribute(new ClassAttribute
                        {
                            Name = column.Text,
                            Column = column,
                            Parent = entity,
                            Property = cm.IdentifierProperty,
                            OwningClass = cm
                        });
                }

                foreach (var property in cm.PropertyIterator.Where(p => p.ColumnIterator.Any()))
                {
                    var column = property.ColumnIterator.First() as Column;
                    var a = new ClassAttribute
                    {
                        Name = column.Text,
                        Property = property,
                        Parent = entity,
                        Column = column,
                        OwningClass = cm
                    };

                    if (a.Property.IsEntityRelation)
                    {
                        a.ReferencedEntity = GetTypeMap()[a.Property.Type.ReturnedClass];
                        if (a.ReferencedEntity == null)
                        {
                            RegisterProblem(new Problem
                                    {
                                        Location = a,
                                        Solution = "ADD MAPPING",
                                        Description = "Referenced class not mapped",
                                    });
                        }
                    }
                    entity.AddAttribute(a);
                }

            }

            foreach (var fk in entity.MappingTable.ForeignKeyIterator)
            {
                // currently only supports keys to non-composite id's
                var column = fk.ColumnIterator.First();

                // it's already there, appearantly its also a reference
                var attribute = entity.GetAttributeByName(column.Text);

                if (attribute == null)
                {
                    attribute = new Attribute
                    {
                        Name = column.Name,
                        Parent = entity,
                        ReferencedEntity = Model.GetEntityByName(fk.ReferencedTable.Name),
                        Column = column
                    };
                    entity.AddAttribute(attribute);
                }
            }
        }
    }
}
