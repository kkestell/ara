#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Expressions;

public record CallExpression(IParseNode Node, string Name, NodeList<Argument> Arguments) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Arguments };

    public override Type Type
    {
        get
        {
            var func = (FunctionDefinition?)NearestAncestor<SourceFile>().Definitions.Nodes.SingleOrDefault(x => x is FunctionDefinition f && f.Name == Name);

            if (func is null)
                throw new ReferenceException(this);

            return func.Type;
        }
    }
}
