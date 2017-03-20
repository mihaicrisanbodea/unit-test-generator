using System;
using System.Collections.Generic;

namespace TestsGenerator.Models
{
    public class MethodProperties
    {
        public string Name { get; set; }

        public Type ReturnType { get; set; }

        public List<Parameter> Parameters { get; set; }
    }
}
