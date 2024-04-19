using Ara.Parsing.Nodes;

namespace Ara.Semantics;

public class UfcsVisitor
{
    public void Visit(Node node)
    {
        foreach (var child in node.Children)
        {
            Visit(child);
        }

        if (node is not CallExpression { Callee: MemberAccessExpression(var left, var right, _) } call)
        {
            return;
        }
        
        var args = new List<Argument> { new(left, call.Location) };
        args.AddRange(call.Arguments);

        call.Callee = right;
        call.Arguments = args;
    }
}