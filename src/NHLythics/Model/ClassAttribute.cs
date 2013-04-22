using System.Linq;
using NHibernate.Mapping;

namespace NHLythics.Model
{
    public class ClassAttribute : Attribute
    {
        public PersistentClass OwningClass { get; set; }

        public Property Property { get; set; }

        public override Column Column {
            get
            {
                if (Property == null)
                    return null;
                return Property.ColumnIterator.Single() as Column;
            }
        }


        public bool IsOfSubclassAndThusOptionalByDefinition
        {
            get { return OwningClass.Superclass != null; }
        }

        public bool IsReference
        {
            get { return OwningClass.GetProperty(Name).IsEntityRelation; }
        }

        public override bool IsOptional
        {
            get { return IsOfSubclassAndThusOptionalByDefinition || base.IsOptional; }
        }
    }
}