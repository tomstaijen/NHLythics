using System.Linq;
using NHibernate.Mapping;

namespace NHLythics.Model
{
    public class Attribute : Element
    {
        public string Name { get; set; }
        public Property Property { get; set; }
        public Element Parent { get; set; }

        public Entity ReferencedEntity { get; set; }
        
        public Column Column { 
            get
            {
                if (Property == null)
                    return null;
                return Property.ColumnIterator.Single() as Column;
            }
        }

        public override string QualifiedName { get { return string.Format("{0}.{1}", Parent.Name, Name); } }

        public virtual bool IsOptional
        {
            get { return Column.IsNullable; }
        }

        public string Type
        {
            get { return Column.SqlType; }
        }
    }
}