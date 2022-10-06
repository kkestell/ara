using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Statements;

public record VariableDeclaration(IParseNode Node, string Name, TypeRef? TypeRef, Expression? Expression) : Statement(Node), ITyped
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode?> { TypeRef, Expression }
        .Where(x => x is not null).Select(x => x as AstNode)!;
    
    public Type Type
    {
        get
        {
            if (TypeRef is not null)
                return TypeRef.ToType();

            if (Expression is not ITyped i)
                throw new SemanticException(this, "Cannot infer type of non-constant values");

            return i.Type;
        }
    }
}
