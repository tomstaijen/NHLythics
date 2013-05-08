using System.Linq;
using NHibernate.Mapping;

namespace NHLythics.Model
{
    public class Attribute : Element
    {
        public Element Parent { get; set; }
        
        public Entity ReferencedEntity { get; set; }

        public virtual Column Column { get; set; }


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

    public class DiscriminatorAttribute : ClassAttribute
    {
        public override bool IsOptional
        {
            get { return false; }
        }
    }
}