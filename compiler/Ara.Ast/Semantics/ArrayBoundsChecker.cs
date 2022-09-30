using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Semantics.Types;

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

    void CheckBounds(ArrayIndex i)
    {
        if (i.VariableReference.Type is not ArrayType t)
            throw new SemanticException(i, $"Attempting to index something that isn't an array!");

        if (i.Index.Type is not IntegerType)
            throw new SemanticException(i, $"Array index must be an integer, not {i.Index.Type}");

        if (i.Index is not Constant c)
            throw new SemanticException(i, $"Array index must be a constant value");

        var idx = int.Parse(c.Value);

        if (idx > t.Size - 1)
            throw new ArrayIndexOutOfBoundsException(i);
    }
}
