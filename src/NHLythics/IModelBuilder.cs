using System.Collections.Generic;
using NHLythics.Model;

namespace NHLythics
{
    public interface IModelBuilderExtension
    {
        IEnumerable<Problem> Build(MappingModel model);
    }
}