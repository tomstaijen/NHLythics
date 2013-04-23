﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHLythics.Model;
using NHLythics.Processors;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHLythics
{
    public class ModelBuilder
    {
        private List<Action<ModelChecker>> _actions = new List<Action<ModelChecker>>();

        public ICollection<Action<ModelChecker>> Actions { get { return _actions;  } }

        public void ApplyMappings(Configuration configuration)
        {
            _actions.Add(c =>
                {
                    c.Apply(new EntityHarvester { Configuration = configuration });
                    c.Apply(new AttributeHarvester());
                });
        }

        public void ApplyDatabase(string connectionString)
        {
            _actions.Add(c =>
                {
                    c.Apply(new TableFinder {ConnectionString = connectionString});
                    c.Apply(new TableValidator());
                });
        }
    }
}
