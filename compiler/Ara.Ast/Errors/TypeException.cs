using Ara.Ast.Nodes.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Errors;

public class TypeException : SemanticException
{
    public TypeException(AstNode node, Type firstType, Type secondType) : base(node, BuildMessage(firstType, secondType))
    {
    }

    private static string BuildMessage(Type firstType, Type secondType)
    {
        return $"Invalid type {firstType} where {secondType} was expected.";
    }
}