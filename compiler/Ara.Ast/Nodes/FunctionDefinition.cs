using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(IParseNode Node, string Name, TypeRef? ReturnTypeRef, NodeList<Parameter> Parameters, Block Block) : AstNode(Node), ITyped
{
    List<AstNode>? children;

    public override IEnumerable<AstNode> Children
    {
        get
        {
            if (children is not null)
                return children;

            children = new List<AstNode> { Parameters, Block };

            if (ReturnTypeRef is not null)
                children.Add(ReturnTypeRef);

            return children;
        }
    }

    public Type Type { get; set; } = Type.Unknown;

    public List<Return> Returns { get; } = new();
}
