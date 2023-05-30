#region

using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes;

public record ExternalFunctionDeclaration(IParseNode Node, string Name, NodeList<Parameter> Parameters, TypeRef ReturnTypeRef) : AstNode(Node)
{
    private List<AstNode>? _children;

    public override IEnumerable<AstNode> Children
    {
        get
        {
            if (_children is not null)
                return _children;

            _children = new List<AstNode>
            {
                Parameters,
                ReturnTypeRef
            };

            return _children;
        }
    }

    public Type Type { get; } = ReturnTypeRef.ToType();
}