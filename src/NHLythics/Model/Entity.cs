﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DatabaseSchemaReader.DataSchema;
using NHibernate.Mapping;
using NHLythics.Extensions;

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
        private readonly Dictionary<string,Attribute> _attributes = new Dictionary<string, Attribute>();

        public List<PersistentClass> Classes { get { return _classes; } }
        public List<Collection> Collections { get { return _collections; } }
        public ReadOnlyCollection<Attribute> Attributes { get { return _attributes.Values.ToList().AsReadOnly(); } }

        /// <summary>
        /// The attribute that holds the identity. Currently, we only support non-composite identity's.
        /// </summary>
        public Attribute IdentityAttribute { 
            get
            {
                if (MappingTable.HasPrimaryKey)
                {
                    return
                        Attributes.SingleOrDefault(a => a.Name == MappingTable.PrimaryKey.ColumnIterator.First().Name);
                }
                return null;
            }
        }

        public Table MappingTable
        {
            get
            {
                if (_classes.Any())
                    return _classes.First().Table;
                if (_collections.Any())
                    return _collections.First().CollectionTable;
                return null;
            }
        }

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

        public void AddAttribute(Attribute a)
        {
            _attributes.Add(a.Name, a);
        }


        public Attribute GetAttributeByName(string name)
        {
            return _attributes.GetValueOrDefault(name);
        }

    }
}