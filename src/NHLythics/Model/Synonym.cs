using System.Linq;

namespace NHLythics.Model
{
    public class Synonym
    {
        public string Name { get; set; }
        public string Target { get; set; }

        public string Database { get { return Target.Split('.')[0]; } }
        public string Schema { get { return Target.Split('.')[1]; } }
        public string Table { get { return Target.Split('.').Last(); } }
    }
}