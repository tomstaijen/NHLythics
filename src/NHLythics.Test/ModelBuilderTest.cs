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

            var builder = new ModelBuilder(Configuration);

            // assert
            builder.Should().BeNull();
        }

    }
}
