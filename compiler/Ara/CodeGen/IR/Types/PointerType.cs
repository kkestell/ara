namespace Ara.CodeGen.IR.Types;

public class PointerType : IrType
{
    public IrType Type { get; }

    public PointerType(IrType type)
    {
        Type = type;
    }

    public override string ToIr()
    {
        return "ptr";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not PointerType other)
            return false;

        return other.Type.Equals(Type);
    }
}