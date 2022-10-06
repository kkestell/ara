using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions;

public record Call(IParseNode Node, string Name, NodeList<Argument> Arguments) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Arguments };

    public override Type Type
    {
        get
        {
            var func = NearestAncestor<SourceFile>().FunctionDefinitions.Nodes.SingleOrDefault(x => x.Name == Name);

            if (func is null)
                throw new ReferenceException(this);

            return func.Type;
        }
    }
}
