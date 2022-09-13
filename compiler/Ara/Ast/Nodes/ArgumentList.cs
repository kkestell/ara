using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class ArgumentList : AstNode
{
    public ArgumentList(Node node, IEnumerable<Argument> arguments) : base(node)
    {
        Arguments = arguments;
    }

    public IEnumerable<Argument> Arguments { get; }
}
