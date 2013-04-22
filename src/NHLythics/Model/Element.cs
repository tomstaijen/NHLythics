namespace NHLythics.Model
{
    public class Element
    {
        public string Name { get; set; }

        public virtual string QualifiedName
        {
            get { return Name; }
        }
    }

}