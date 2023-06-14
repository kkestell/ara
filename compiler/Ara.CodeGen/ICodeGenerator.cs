using Ara.Ast.Nodes;

namespace Ara.CodeGen;

public interface ICodeGenerator
{
    string Generate(SourceFile root);
}