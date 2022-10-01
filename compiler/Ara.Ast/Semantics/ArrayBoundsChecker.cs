using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Types;

namespace Ara.Ast.Semantics;

public class ArrayBoundsChecker : Visitor
{
    public ArrayBoundsChecker(SourceFile sourceFile) : base(sourceFile)
    {
    }
    
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case ArrayIndex i:
                CheckBounds(i);
                break;
        }
    }

    static void CheckBounds(ArrayIndex arrayIndex)
    {
        if (arrayIndex.VariableReference.Type is not ArrayType t)
            throw new SemanticException(arrayIndex, "Attempting to index something that isn't an array!");

        if (arrayIndex.Index is not IntegerValue integer)
            throw new SemanticException(arrayIndex, "Array index must be an integer");

        if (integer.Value > t.Size - 1)
            throw new ArrayIndexOutOfBoundsException(arrayIndex);
    }
}
