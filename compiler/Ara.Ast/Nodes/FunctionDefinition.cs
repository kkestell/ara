using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Types;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes;

public record FunctionDefinition(IParseNode Node, string Name, NodeList<Parameter> Parameters, TypeRef? ReturnTypeRef, Block Block) : AstNode(Node), ITyped
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
    
    public Type Type
    {
        get
        {
            if (ReturnTypeRef is not null)
                return ReturnTypeRef.ToType();

            var returns = Descendants<Return>().ToList();

            if (returns.Count == 0)
                return Type.Void;

            var returnTypes = returns
                .Select(x => x.Expression.Type)
                .Where(x => x is not UnknownType)
                .ToList();

            if (returnTypes.Distinct().Count() > 1)
                throw new SemanticException(this, "Function returns more than one type");

            return returnTypes.First();
        }
    }
}
