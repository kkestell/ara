using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;

namespace Ara.Ast.Semantics;

public class TypeResolver : Visitor
{
    public TypeResolver(SourceFile sourceFile) : base(sourceFile)
    {
    }

    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case Return r:
                ResolveReturn(r);
                break;
        }
    }

    // FIXME: Move this to AstBuilder
    static void ResolveReturn(Return r)
    {
        //r.NearestAncestor<FunctionDefinition>().Returns.Add(r);
    }
}
