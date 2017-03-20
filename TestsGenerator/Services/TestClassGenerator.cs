using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestsGenerator.Models;

namespace TestsGenerator.Services
{
    public class TestClassGenerator
    {
        private const string TestExtension = "Tests";
        private readonly MemberGenerator memberGenerator = new MemberGenerator();

        public void Generate(ClassProperties classProperties)
        {
            var testClassNamespace = GetTestName(classProperties.Namespace);
            var nameSpace = memberGenerator.GetNamespace(testClassNamespace);

            var testClassName = GetTestName(classProperties.Name);
            var testClass = memberGenerator.GetClass(testClassName, MemberAttributes.Public);

            foreach (var dependency in classProperties.Dependencies)
            {
                memberGenerator.AddMembers(testClass, new List<Member>
                {
                    new Member
                    {
                        Type = dependency.Type,
                        Name = dependency.Name
                    }
                });
            }

            foreach (var method in classProperties.Methods)
            {
                memberGenerator.AddMethodUnderTest(testClass, method, classProperties.Type);
            }

            memberGenerator.AddBuildTargetMethod(testClass, classProperties.Type,
                classProperties.Dependencies.Select(s => s.Name).ToList());

            nameSpace.Types.Add(testClass);
            WriteTestClass(nameSpace, classProperties.FullName);
        }

        private static void WriteTestClass(CodeNamespace codeNamespace, string filePath)
        {
            var myassembly = new CodeCompileUnit();
            myassembly.Namespaces.Add(codeNamespace);
            Microsoft.CSharp.CSharpCodeProvider ccp = new Microsoft.CSharp.CSharpCodeProvider();
            var tw1 = new IndentedTextWriter(new StreamWriter(filePath, false), "    ");
            ccp.GenerateCodeFromCompileUnit(myassembly, tw1, new CodeGeneratorOptions());
            tw1.Close();
        }

        private static string GetTestName(string name)
        {
            return $"{name}{TestExtension}";
        }
    }
}
