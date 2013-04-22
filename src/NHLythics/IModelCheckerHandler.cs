using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLythics.Model;

namespace NHLythics
{
    public interface IModelCheckerHandler
    {
        void Notify(Problem problem);
    }
}
