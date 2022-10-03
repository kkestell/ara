using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions;

public record Call(IParseNode Node, string Name, NodeList<Argument> Arguments) : Expression(Node)
{
    readonly AstNode[] children = { Arguments };

    public override IEnumerable<AstNode> Children => children;

    public override Type Type
    {
        get
        {
            var func = NearestAncestor<SourceFile>().FunctionDefinitions.Nodes.SingleOrDefault(x => x.Name == Name);

            if (func is null)
                throw new ReferenceException(this);

            return func.Type;
        }

        set => throw new NotSupportedException();
    }
}
