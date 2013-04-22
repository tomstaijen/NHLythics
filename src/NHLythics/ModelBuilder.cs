using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using NHLythics.Model;
using NHLythics.Processors;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHLythics
{
    public class ModelBuilder
    {
        private ModelChecker _checker;
        
        private List<Action> _actions = new List<Action>();

        public ModelBuilder(ModelChecker checker)
        {
            _checker = checker;
        }

        public void ApplyMappings(Configuration configuration)
        {
            _actions.Add(() =>
                {
                    _checker.Apply(new EntityHarvester { Configuration = configuration });
                    _checker.Apply(new AttributeHarvester());
                });
        }

        public void ApplyDatabase(string connectionString)
        {
            _actions.Add(() => _checker.Apply(new TableFinder { ConnectionString = connectionString }));
        }
    }
}
