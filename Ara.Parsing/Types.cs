using Ara.Parsing.Nodes;

namespace Ara.Parsing.Types;

public abstract record Type;

public record UnknownType : Type
{
    public override string ToString()
    {
        return "unknown";
    }
}

public record VoidType : Type
{
    public override string ToString()
    {
        return "void";
    }
}

public record IntType(int SizeBits, bool IsSigned = true) : Type
{
    public override string ToString()
    {
        var prefix = IsSigned ? "i" : "u";
        return $"{prefix}{SizeBits}";
    }
}

public record FloatType(int SizeBits) : Type
{
    public override string ToString()
    {
        return $"f{SizeBits}";
    }
}

public record BoolType : Type
{
    public override string ToString()
    {
        return "bool";
    }
}

public record StringType : Type
{
    public override string ToString()
    {
        return "string";
    }
}

public record ArrayType(Type ElementType, int Size) : Type
{
    public override string ToString()
    {
        return $"[{Size}]{ElementType}";
    }
}

public record FunctionType(Type ReturnType, IReadOnlyList<Type> ParameterTypes) : Type
{
    public override string ToString()
    {
        return $"({string.Join(", ", ParameterTypes)}) => {ReturnType}";
    }
}

public record StructType(string Name) : Type
{
    public Dictionary<string, Type> Fields { get; set; } = new();
    
    public override string ToString()
    {
        return $"struct {Name}";
    }
}

public record PointerType(Type ElementType) : Type
{
    public override string ToString()
    {
        return $"^{ElementType}";
    }
}
