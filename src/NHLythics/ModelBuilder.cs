using System;
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
using NHLythics.Extensions;
using NHibernate.Mapping;

namespace NHLythics
{
    public class ModelBuilder
    {
        private List<Action<ModelChecker>> _actions = new List<Action<ModelChecker>>();

        public ICollection<Action<ModelChecker>> Actions { get { return _actions;  } }
        
        public void AddAction(Action<ModelChecker> action)
        {
            _actions.Add(action);
        }

        public void UseConfiguration(Configuration configuration)
        {
            configuration.BuildMappings();
            _actions.Add(c =>
                {
                    c.Configuration = configuration;
                    c.Apply(new EntityHarvester { Configuration = configuration });
                    c.Apply(new AttributeHarvester());
                });
        }

        public void LoadMappingsFrom(string directory, string filepattern = "*.dll")
        {
            var assemblyFiles = Directory.GetFiles(directory, filepattern);
            var assemblies = assemblyFiles.Select(f => Assembly.LoadFrom(f)).ToArray();
            UseConfigurationWithMappingsFromAssemblies(assemblies);

            var lookup = assemblies.ToDictionary(a => a.FullName, a => a);
            AppDomain.CurrentDomain.AssemblyResolve += (o, aR) =>
                {
                    return lookup.GetValueOrDefault(aR.Name);
                };
        }

        public void UseConfigurationWithMappingsFromAssemblies(Assembly[] assemblies)
        {
            var config = Fluently.Configure()
                                 .Database(MsSqlConfiguration.MsSql2008)
                                 .Mappings(m =>
                                 {
                                     foreach (var a in assemblies)
                                     {
                                         m.FluentMappings.AddFromAssembly(a);
                                     }
                                 })
                                 .BuildConfiguration();
            UseConfiguration(config);
        }

        public void ApplyDatabase(string connectionString)
        {
            _actions.Add(c =>
                {
                    c.Apply(new TableFinder {ConnectionString = connectionString});
                    c.Apply(new TableValidator(c.Dialect, c.Mapping));
                });
        }
    }
}
