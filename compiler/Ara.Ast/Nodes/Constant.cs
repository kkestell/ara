using Ara.Ast.Semantics.Types;
using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public interface ITyped
{
    public Type Type { get; set; }
}

public record IntegerValue(Node Node, int Value) : Atom(Node), ITyped
{
    public override Type Type
    {
        get => new IntegerType();
        set => throw new NotImplementedException();
    }
}

public record FloatValue(Node Node, float Value) : Atom(Node), ITyped
{
    public override Type Type
    {
        get => new FloatType();
        set => throw new NotImplementedException();
    }
}

public record BooleanValue(Node Node, bool Value) : Atom(Node), ITyped
{
    public override Type Type
    {        
        get => new BooleanType();
        set => throw new NotImplementedException();
    }
}