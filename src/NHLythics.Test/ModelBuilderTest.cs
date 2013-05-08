using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NHLythics.FluentLoading;
using NHLythics.Model;
using NUnit.Framework;

namespace NHLythics.Test
{
    [TestFixture]
    public class ModelBuilderTest : TestBase
    {
        /// <summary>
        /// 
        /// ***LOADING***
        /// 
        /// 1. Load Model from Configuration
        ///    a. Collect entities
        ///    b. Harvest attributes               => Report unknown types, report non-virtual public/protected stuffs (optional?)
        /// 
        /// 2. Check for synonyms (is done on the fly)
        /// 
        /// 3. (DB) Find tables for entities       => Report missing table, Report extra tables
        ///    (DB) a. Find synonyms for synonyms  => Report missing synonym
        ///    (DB) b. Find tables for synonyms    => Report missing table
        /// 
        /// ***CHECKING***
        /// 
        /// B. (DB) Check column types
        /// C. (DB) Check for problematic extra columns
        /// D. (DB) Check missing foreign keys
        /// E. (DB) Check for foreign keys to erasable data
        /// 
        /// ***OUTPUT***
        /// I. Print table info
        /// II. Print createscript
        /// III. Print problems with severity
        /// IV. 
        /// </summary>
        [Test]
        public void CanBuildModel()
        {
            DefaultArrange();

            var assemblies = new [] { typeof (NControl.Organisaties.Model.Organisaties.Apotheek).Assembly } ;

            var checker = ModelChecker.Build(b =>
                {
                    b.LoadMappingsFrom(@"C:\Users\tom\Repositories\NControl\Source\NControl\NControl.NHibernate\bin\Release", "NControl.*.dll");
                    //b.UseConfigurationWithMappingsFromAssemblies(assemblies);
                    b.ApplyDatabase(GetConnectionString("192.168.1.54", "NSafe", "sa","iSaTiS1900"));
                });

            checker.Validate();

            checker.Problems.ForEach(p => Console.WriteLine(p));


            // assert
            Assert.That(!checker.Problems.Any());
        }
    }
}
