using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLythics.Extensions;
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
                if (cm.Discriminator != null)
                {
                    var column = cm.Discriminator.ColumnIterator.First() as Column;
                    
                    if (entity.GetAttributeByName(column.Name) == null) // add just once, all subclasses also have the discriminator
                        entity.AddAttribute(new DiscriminatorAttribute
                            {
                                Name = column.Name,
                                Parent = entity,
                                OwningClass = cm,
                                Column = column
                            });
                }

                // TODO: put this into a union with the propertyiterator
                if (cm.HasIdentifierProperty && cm.IdentifierProperty.PersistentClass == cm)
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
                else if (cm.Identifier is Component)
                {
                    foreach (var property in (cm.Identifier as Component).PropertyIterator)
                    {
                        HandleProperty(entity, cm, property);
                    }
                }

                foreach (var property in cm.PropertyIterator.Where(p => p.ColumnIterator.Any()))
                {
                    HandleProperty(entity, cm, property);
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
                else
                {
                    // TODO: this should be a ClassAttribute that references some type, check that?
                }
            }
        }


        public void HandleProperty(Entity entity, PersistentClass cm, Property property, Component component = null)
        {
            if (property.IsComposite)
            {
                var comp = property.Value as Component;
                foreach (var p in comp.PropertyIterator.Where(p => p.ColumnIterator.Any()))
                {
                    HandleProperty(entity, cm, p, comp);
                }
                return;
            }

            dynamic column = property.ColumnIterator.First();
            if (column is Formula)
            {
                // todo: what to do with these?
                return;
            }

            column = column as Column;

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
                a.ReferencedEntity = GetTypeMap().GetValueOrDefault(a.Property.Type.ReturnedClass);
                if (a.ReferencedEntity == null)
                {
                    RegisterProblem(ProblemType.UnknownClass, Severity.Critical, a);
                }
            }

            var attr = entity.GetAttributeByName(a.Name);
            if ( attr != null)
            {
                RegisterProblem(ProblemType.DoubleProperty, Severity.Important, attr,
                                "Also found in " + cm.MappedClass.Name + ", can be bad when types differ.");
            }
            else
            {
                entity.AddAttribute(a);
            }
        }
    }
}
