using System.CodeDom;

namespace TestsGenerator.Extensions
{
    public static class CodeMemberMethodExtensions
    {
        public static void AddEmptyLine(this CodeMemberMethod method)
        {
            method.Statements.Add(new CodeSnippetStatement(string.Empty));
        }

        public static void AddComment(this CodeMemberMethod method, string comment)
        {
            method.Statements.Add(new CodeCommentStatement(comment, false));
        }
    }
}
