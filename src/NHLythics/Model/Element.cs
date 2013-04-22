﻿using System.Diagnostics;

namespace NHLythics.Model
{
    [DebuggerDisplay("Element Name = {QualifiedName}")]
    public class Element
    {
        public string Name { get; set; }

        public virtual string QualifiedName
        {
            get { return Name; }
        }
    }

}