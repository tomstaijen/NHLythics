using System.Diagnostics;

namespace NHLythics.Model
{
    [DebuggerDisplay("{Severity} - {Type} - {Location.QualifiedName} - {Details}")]
    public class Problem
    {
        public Element Location { get; set; }
        public Severity Severity { get; set; }
        public ProblemType Type { get; set; }
        public string Details { get; set; }

        public override string ToString()
        {
            if (Location == null)
                return string.Format("{0} - {1} - {2}", Severity, Type, Details);
            return string.Format("{0} - {1} - {2} - {3}", Severity, Type, Location.QualifiedName, Details);
        }

        public static Problem Create(ProblemType type, Severity severity, Element location, string details = null)
        {
            return new Problem
                {
                    Location = location,
                    Severity = severity,
                    Type = type,
                    Details = details
                };
        }
    }

    public enum ProblemType : short
    {
        None = 0,
        MissingTable = 1,
        MissingColumn = 2,
        ColumnTypeMismatch = 3,
        ExtraColumn = 4,
        ExtraColumnNotNullable = 5,
        MissingSynonym = 6,
        MissingView = 7,
        MissingKey = 8,
        ExtraKey = 9,
        ExtraTable = 10,
        UnknownClass = 11,
        DoubleProperty = 12,
        IncompleteMapping = 13
    }

    public enum Severity : short
    {
        None = 0,

        /// <summary>
        /// FYI's
        /// Unknown tables.
        /// Unknown Indexes
        /// Unknown Keys
        /// </summary>
        Low = 10,

        /// <summary>
        /// Should be cleaned up some day.
        /// Extra columns that are not used.
        /// </summary>
        Moderate = 20,

        /// <summary>
        /// Severy impact on the performance or the integrity of the system.
        /// Missing foreign keys, missing indexes
        /// </summary>
        Important = 30,

        /// <summary>
        /// System will crash without this fixed
        /// Structure is not complete or not compatible.
        /// </summary>
        Critical = 40
    }
}