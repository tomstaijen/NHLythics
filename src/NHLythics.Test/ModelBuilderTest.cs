using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
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

            var model = new ModelBuilder(Configuration).Build();

            model.AnalyseClasses();

            // assert
            model.Should().NotBeNull();
        }
    }
}
