namespace Ara.Ast.Types;

public class InferredType
{
    public InferredType(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not InferredType other)
            return false;

        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
