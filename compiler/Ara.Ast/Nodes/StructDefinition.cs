#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes;

public record StructDefinition(IParseNode Node, string Name, NodeList<StructField> Fields) : Definition(Node, Name)
{
    private List<AstNode>? _children;

    public override IEnumerable<AstNode> Children
    {
        get
        {
            if (_children is not null)
                return _children;

            _children = new List<AstNode> { Fields };

            return _children;
        }
    }
}