using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestsGenerator.Services;

namespace TestsGenerator
{
    public class GenerateTestMethods
    {
        public static void Main(string[] args)
        {
            var targetAssemblyLocation = args[0];
            var validator = new ValidatorService();

            if (!validator.IsAssemblyPathValid(targetAssemblyLocation))
            {
                Console.WriteLine("Assembly path not valid {0}", targetAssemblyLocation);
                return;
            }

            var classes = GetClasses(targetAssemblyLocation);
            var classPropertiesService = new ClassPropertiesService();
            var testClassGenerator = new TestClassGenerator();

            foreach (var assemblyClass in classes)
            {
                var classProperties = classPropertiesService.GetClassProperties(assemblyClass);
                if (classProperties != null && classProperties.Methods.Any())
                {
                    testClassGenerator.Generate(classProperties);
                }
            }
        }

        private static List<Type> GetClasses(string assemblyLocation)
        {
            return GetTypes(assemblyLocation)
                .Where(t => t.IsClass)
                .Where(t => !t.Name.StartsWith("<"))
                .ToList();
        }

        private static List<Type> GetTypes(string assemblyLocation)
        {
            var assembly = Assembly.LoadFrom(assemblyLocation);

            try
            {
                return assembly.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).ToList();
            }
        }

    }
}
