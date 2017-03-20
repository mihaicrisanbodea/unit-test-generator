using System;
using System.Collections.Generic;

namespace TestsGenerator.Models
{
    public class ClassProperties
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public List<Parameter> Dependencies { get; set; }

        public List<MethodProperties> Methods { get; set; }

        public List<string> Usings { get; set; }

        public Type Type { get; set; }
    }
}
