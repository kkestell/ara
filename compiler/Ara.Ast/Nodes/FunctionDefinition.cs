#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes;

public record FunctionDefinition(IParseNode Node, string Name, NodeList<Parameter> Parameters, TypeRef? ReturnTypeRef, Block Block) : AstNode(Node)
{
    private List<AstNode>? _children;

    public override IEnumerable<AstNode> Children
    {
        get
        {
            if (_children is not null)
                return _children;

            _children = new List<AstNode> { Parameters, Block };

            if (ReturnTypeRef is not null)
                _children.Add(ReturnTypeRef);

            return _children;
        }
    }

    public Type Type { get; set; } = Type.Unknown;

    public List<Return> Returns { get; } = new();
}
