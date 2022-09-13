using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public class FunctionCallExpression : Expression
{
    public FunctionCallExpression(Node node, Identifier name, List<Argument> arguments) : base(node)
    {
        Name = name;
        Arguments = arguments;
    }
    
    public Identifier Name { get; }
    
    public List<Argument> Arguments { get; }
}
