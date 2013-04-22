using System.Diagnostics;

namespace NHLythics.Model
{
    [DebuggerDisplay("Location = {Location.QualifiedName}, Problem = {Description}")]
    public class Problem
    {
        public Element Location { get; set; }
        public string Description { get; set; }
        
        /// <summary>
        /// Returns a script that fixes the problem
        /// </summary>
        public string Solution { get; set; }
    }

    public enum Severity
    {
        None,

        /// <summary>
        /// FYI's
        /// Unknown tables.
        /// Unknown Indexes
        /// Unknown Keys
        /// </summary>
        Low,

        /// <summary>
        /// Should be cleaned up some day.
        /// Extra columns that are not used.
        /// </summary>
        Moderate,

        /// <summary>
        /// Severy impact on the performance or the integrity of the system.
        /// Missing foreign keys, missing indexes
        /// </summary>
        Important,

        /// <summary>
        /// System will crash without this fixed
        /// Structure is not complete or not compatible.
        /// </summary>
        Critical
    }

    [DebuggerDisplay("Unknown table {Location.Name}")]
    public class UnknownTableProblem : Problem
    {
    }
}