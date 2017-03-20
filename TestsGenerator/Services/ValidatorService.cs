using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestsGenerator.Services
{
    public class ValidatorService
    {
        private readonly List<string> ValidAssemblyExtensions = new List<string> {".dll", ".exe"};

        public bool IsAssemblyPathValid(string path)
        {
            return !string.IsNullOrWhiteSpace(path) &&
                   IsFileExtensionValid(path) && 
                   File.Exists(path);
        }

        private bool IsFileExtensionValid(string path)
        {
            return ValidAssemblyExtensions.Any(validAssemblyExtension =>
                path.EndsWith(validAssemblyExtension, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
