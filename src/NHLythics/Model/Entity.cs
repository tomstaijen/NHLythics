using System.Collections.Generic;
using System.Linq;
using DatabaseSchemaReader.DataSchema;
using NHibernate.Mapping;

namespace NHLythics.Model
{
    /// <summary>
    /// Wrapper for PersistentClasses and Collections that are mapped to the same Table. All this holds the information to analyse the correctness of the table versus the mapping.
    /// </summary>
    public class Entity : Element
    {
        private MappingModel _model;
        public Entity(MappingModel model)
        {
            _model = model;
        }

        public MappingModel Model { get { return _model;  } }

        private readonly List<PersistentClass> _classes = new List<PersistentClass>();
        private readonly List<Collection> _collections = new List<Collection>();
        private readonly List<Attribute> _attributes = new List<Attribute>();

        public List<PersistentClass> Classes { get { return _classes; } }
        public List<Collection> Collections { get { return _collections; } }
        public List<Attribute> Attributes { get { return _attributes; } }

        public DatabaseTable Table { get; set; }

        public bool InMapping
        {
            get { return _classes.Any() || _collections.Any(); }
        }

        public bool IsSynonym
        {
            get
            {
                if (Classes.Any())
                    return ModelRegistry.IsSynonym(Classes.First().MappedClass);
                return false;
            }
        }

        public Synonym Synonym
        {
            get
            {
                if (Classes.Any())
                {
                    return ModelRegistry.GetSynonym(Classes.First().MappedClass);
                }
                return null;
            }
        }


    }
}