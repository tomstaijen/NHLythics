using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLythics.Model;

namespace NHLythics.Handlers
{
    class ProblemCollecter : IModelCheckerHandler
    {
        public ProblemCollecter()
        {
            Problems = new List<Problem>();
        }

        public List<Problem> Problems { get; private set; }

        public void Notify(Problem problem)
        {
            Problems.Add(problem);
        }
    }
}
