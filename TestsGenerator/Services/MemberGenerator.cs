using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using TestsGenerator.Extensions;
using TestsGenerator.Models;

namespace TestsGenerator.Services
{
    public class MemberGenerator
    {
        private const string BuildTargetMethodName = "BuildTarget";
        private const string TargetPropertyName = "target";
        private const string ActualPropertyName = "actual";
        private const string Arrange = "Arrange";
        private const string Act = "Act";
        private const string Assert = "Assert";

        private const string TestMethodNameExtension = "_Scenario_ExpectedBehavior";

        public CodeNamespace GetNamespace(string nameSpace)
        {
            return new CodeNamespace(nameSpace);
        }

        public CodeTypeDeclaration GetClass(string className, MemberAttributes memberAttribute)
        {
            return new CodeTypeDeclaration
            {
                Name = className,
                IsClass = true,
                Attributes = memberAttribute
            };
        }

        public void AddMembers(CodeTypeDeclaration classDeclaration, List<Member> members)
        {
            foreach (var member in members)
            {
                var codeMember = new CodeMemberField(member.Type, member.Name);
                classDeclaration.Members.Add(codeMember);
            }
        }

        public void AddMethodUnderTest(CodeTypeDeclaration classDeclaration,
            MethodProperties methodProperties,
            Type classType)
        {
            var testMethodName = $"{methodProperties.Name}{TestMethodNameExtension}";
            var method = GetCodeMemberMethod(testMethodName, true);

            method.AddComment(Arrange);
            DeclareParameters(method, methodProperties.Parameters);
            DeclareMemberInstance(method, classType);
            method.AddEmptyLine();

            method.AddComment(Act);
            InvokeMethodWithParameters(method, methodProperties);

            classDeclaration.Members.Add(method);
            method.AddEmptyLine();
            method.AddComment(Assert);
        }

        public void AddBuildTargetMethod(CodeTypeDeclaration classDeclaration,
            Type returnType,
            List<string> dependencyNames)
        {
            var expressions = new List<CodeExpression>();

            if (dependencyNames != null)
            {
                expressions.AddRange(dependencyNames.Select(dependencyName => new CodeFieldReferenceExpression(null, dependencyName)));
            }

            var method = GetCodeMemberMethod(BuildTargetMethodName, false);

            if (returnType != null)
            {
                method.ReturnType = new CodeTypeReference(returnType);
                method.Statements.Add(new CodeMethodReturnStatement(new CodeObjectCreateExpression(returnType, expressions.ToArray())));
            }

            classDeclaration.Members.Add(method);
        }

        private static void DeclareMemberInstance(CodeMemberMethod method, Type classType)
        {
            method.Statements.Add(new CodeVariableDeclarationStatement(classType, TargetPropertyName,
                            new CodeMethodInvokeExpression(null, BuildTargetMethodName)));
        }

        private static void DeclareParameters(CodeMemberMethod method, List<Parameter> parameters)
        {
            foreach (var parameter in parameters.OrderBy(p => p.Position))
            {
                method.Statements.Add(new CodeVariableDeclarationStatement(parameter.Type, parameter.Name,
                    new CodeDefaultValueExpression(new CodeTypeReference(parameter.Type))));
            }
        }

        private static CodeMemberMethod GetCodeMemberMethod(string methodName,
            bool isPublic)
        {
            return new CodeMemberMethod
            {
                Name = methodName,
                Attributes = isPublic
                    ? MemberAttributes.Public | MemberAttributes.Final
                    : MemberAttributes.Private | MemberAttributes.Final
            };
        }

        private void InvokeMethodWithParameters(CodeMemberMethod method, MethodProperties methodProperties)
        {
            var methodParameters = methodProperties.Parameters
                .OrderBy(p => p.Position)
                .Select(p => BuildCodeExpression(p.Name))
                .ToArray();

            var methodCall = new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression(TargetPropertyName),
                methodProperties.Name,
                methodParameters);

            if (methodProperties.ReturnType == typeof(void))
            {
                method.Statements.Add(methodCall);
            }
            else
            {
                method.Statements.Add(new CodeVariableDeclarationStatement(methodProperties.ReturnType,
                    ActualPropertyName, methodCall));
            }
        }

        private CodeExpression BuildCodeExpression(string variableName)
        {
            return new CodeVariableReferenceExpression(variableName);
        }
    }
}
