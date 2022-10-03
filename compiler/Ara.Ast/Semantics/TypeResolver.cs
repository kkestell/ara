using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Types;
using Type = Ara.Ast.Types.Abstract.Type;

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
            case FunctionDefinition f:
                ResolveFunctionDefinition(f);
                break;
        }
    }

    static void ResolveReturn(Return r)
    {
        r.NearestAncestor<FunctionDefinition>().Returns.Add(r);
    }

    static void ResolveFunctionDefinition(FunctionDefinition f)
    {
        if (f.Returns.Count == 0)
        {
            f.Type = Type.Void;
            return;
        }
        
        var returnTypes = f.Returns.Select(x => x.Expression.Type).Where(x => x is not UnknownType).ToList();

        if (returnTypes.Distinct().Count() > 1)
            throw new Exception("Function returns more than one type");

        f.Type = returnTypes.First();
    }
}