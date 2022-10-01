using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Types;

namespace Ara.Ast.Errors;

public class ArrayIndexOutOfBoundsException : SemanticException
{
    public ArrayIndexOutOfBoundsException(ArrayIndex node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(ArrayIndex node)
    {
        var sz = ((ArrayType)node.VariableReference.Type).Size - 1;
        var idx = ((IntegerValue)node.Index).Value;
        return $"Array index {idx} out of bounds (0..{sz}).";
    }
}
