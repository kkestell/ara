using Ara.Parsing.Nodes;

namespace Ara.Semantics;

public class ParentSetter
{
    public void Visit(Node node)
    {
        foreach (var child in node.Children)
        {
            child.Parent = node;
            Visit(child);
        }
    }
}