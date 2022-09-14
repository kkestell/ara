using Ara.Parsing;

namespace Ara.Ast.Nodes;

public abstract record AstNode(Node Node)
{
    public AstNode? _Parent { get; set; }
    
    public T? NearestAncestor<T>() where T : AstNode
    {
        var n = _Parent;
        while (true)
        {
            switch (n)
            {
                case null:
                    return null;
                case T node:
                    return node;
                default:
                    n = n._Parent;
                    break;
            }
        }
    }

    public string ResolveVariableReference(string name)
    {
        var blk = NearestAncestor<Block>();
        while (true)
        {
            if (blk is null)
                break;
            var decl = blk.Statements.SingleOrDefault(s => s is VariableDeclaration d && d.Name.Value == name);
            if (decl is VariableDeclaration v)
                return v.InferredType.Value;
            blk = blk.NearestAncestor<Block>();
        }

        var func = NearestAncestor<FunctionDefinition>()!;
        var param = func.Parameters.FirstOrDefault(p => p.Name.Value == name);
        return param?.Type.Value;
    }
}
