using Ara.Ast.Nodes;
using Ara.Ast.Semantics.Types;

namespace Ara.Ast.Errors;

public class ArrayIndexOutOfBoundsException : SemanticException
{
    public ArrayIndexOutOfBoundsException(ArrayIndex node) : base(node, BuildMessage(node))
    {
    }

    static string BuildMessage(ArrayIndex node)
    {
        var sz = ((ArrayType)node.VariableReference.Type).Size - 1;
        var idx = int.Parse(((Constant)node.Index).Value);
        return $"Array index {idx} out of bounds (0..{sz}).";
    }
}
