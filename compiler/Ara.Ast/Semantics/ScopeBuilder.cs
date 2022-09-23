using Ara.Ast.Nodes;
using Ara.Ast.Semantics.Types;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Semantics;

public class ScopeBuilder : Visitor
{
    public ScopeBuilder(SourceFile sourceFile) : base(sourceFile)
    {
    }
    
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
        f.Block.Scope.Add(f.Counter, new IntegerType());
    }
    
    static void ResolveFunctionDefinition(FunctionDefinition f)
    {
        foreach (var p in f.Parameters)
        {
            f.Block.Scope.Add(p.Name, Type.Parse(p.TypeRef));
        }
    }

    static void ResolveVariableDeclaration(VariableDeclaration d)
    {
        var blk = d.NearestAncestor<Block>();
        blk.Scope.Add(d.Name, Type.Parse(d.TypeRef));
    }
}
