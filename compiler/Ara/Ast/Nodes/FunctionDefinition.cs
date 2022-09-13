using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Ast.Types;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class FunctionDefinition : Definition
{
    public FunctionDefinition(Node node, Identifier name, List<Parameter> parameters, Type_ returnType, Block block) : base(node)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
        Block = block;
    }
    
    public Identifier Name { get; }
    
    public List<Parameter> Parameters { get; }
    
    public Type_ ReturnType { get; }
    
    public Block Block { get; }

    public InferredType InferredType => new InferredType(ReturnType.Value);
}
