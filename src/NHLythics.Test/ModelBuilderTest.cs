using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NHLythics.Model;
using NUnit.Framework;

namespace NHLythics.Test
{
    [TestFixture]
    public class ModelBuilderTest : TestBase
    {
        [Test]
        public void CanBuildModel()
        {
            DefaultArrange();

            var model = MappingModel.Build(b =>
                {
                    b.ApplyMappings(Configuration);
                    //b.ApplyDatabase(GetConnectionString("192.168.0.2", "NSafe", "sa","iSaTiS1900"));
                });

            model.ValidateDatabase();

            // assert
            Assert.That(model.Problems.Any());
        }
    }
}
