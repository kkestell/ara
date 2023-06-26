using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(IParseNode Node, string Name, NodeList<Parameter> Parameters, TypeRef ReturnTypeRef, Block Block) : Definition(Node, Name)
{
    private List<AstNode>? _children;

    public override IEnumerable<AstNode> Children
    {
        get
        {
            if (_children is not null)
                return _children;

            _children = new List<AstNode> { Parameters, Block, ReturnTypeRef };

            return _children;
        }
    }

    public Type Type { get; } = ReturnTypeRef.ToType();
}
