using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestsGenerator.Models;

namespace TestsGenerator.Services
{
    public class ClassPropertiesService
    {
        public ClassProperties GetClassProperties(Type type)
        {
            var ctors = type.GetConstructors();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                              .Where(m => !m.IsSpecialName);

            try
            {
                return new ClassProperties
                {
                    Dependencies = GetParameters(ctors[0]),
                    FullName = type.FullName,
                    Methods = methods.Select(GetMethodProperties).ToList(),
                    Name = type.Name,
                    Namespace = type.Namespace,
                    Type = type
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static MethodProperties GetMethodProperties(MethodInfo methodInfo)
        {
            return new MethodProperties
            {
                Name = methodInfo.Name,
                ReturnType = methodInfo.ReturnType,
                Parameters = GetParameters(methodInfo)
            };
        }

        private static List<Parameter> GetParameters(MethodInfo methodInfo)
        {
            var methodParameters = methodInfo.GetParameters();
            return methodParameters.Select(Map).ToList();
        }

        private static List<Parameter> GetParameters(ConstructorInfo constructorInfo)
        {
            return constructorInfo.GetParameters().Select(Map).ToList();
        }

        private static Parameter Map(ParameterInfo parameter)
        {
            return new Parameter
            {
                Type = parameter.ParameterType,
                Name = parameter.Name,
                Position = parameter.Position
            };
        }
    }
}
