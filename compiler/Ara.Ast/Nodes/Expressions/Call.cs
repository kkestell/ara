using Ara.Ast.Errors;
using Ara.Parsing;
using Type = Ara.Ast.Types.Type;

namespace Ara.Ast.Nodes.Expressions;

public record Call(Node Node, string Name, NodeList<Argument> Arguments) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new() { Arguments };

    public override Type Type
    {
        get
        {
            var func = NearestAncestor<SourceFile>().FunctionDefinitions.Nodes.SingleOrDefault(x => x.Name == Name);

            if (func is null)
                throw new ReferenceException(this);

            return func.ReturnTypeRef.ToType();
        }

        set => throw new NotSupportedException();
    }
}
