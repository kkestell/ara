using Ara.Ast.Nodes;
using Ara.Ast.Types;

namespace Ara.Ast.Semantics;

public class ScopeBuilder : Visitor
{
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case For f:
                ResolveFor(f);
                break;
            case FunctionDefinition f:
                ResolveFunctionDefinition(f);
                break;
            case VariableDeclaration v:
                ResolveVariableDeclaration(v);
                break;
        }
    }
    
    static void ResolveFor(For f)
    {
        f.Block.Scope.Add(f.Counter.Value, new InferredType("int"));
    }
    
    static void ResolveFunctionDefinition(FunctionDefinition f)
    {
        foreach (var p in f.Parameters)
        {
            f.Block.Scope.Add(p.Name.Value, new InferredType(p.Type.Value));
        }
    }

    static void ResolveVariableDeclaration(VariableDeclaration d)
    {
        var blk = d.NearestAncestor<Block>();
        blk.Scope.Add(d.Name.Value, new InferredType(d.Type.Value));
    }
}
