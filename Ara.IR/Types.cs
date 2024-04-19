namespace Ara.IR.Types;

public abstract record IrType;

public record VoidType : IrType
{
    public override string ToString() => "void";
}

public record BooleanType : IrType
{
    public override string ToString() => "i1";
}

public record IntegerType : IrType
{
    public int SizeBits { get; }
    public bool IsSigned { get; }
    
    public IntegerType(int sizeBits, bool isSigned)
    {
        SizeBits = sizeBits;
        IsSigned = isSigned;
    }
    
    public override string ToString() => $"i{SizeBits}";
}

public record FloatType : IrType
{
    public int SizeBits { get; }
    
    public FloatType(int sizeBits)
    {
        SizeBits = sizeBits;
    }

    public override string ToString()
    {
        return SizeBits switch
        {
            32 => "float",
            64 => "double",
            _ => throw new NotImplementedException($"Float size {SizeBits} not implemented")
        };
    }
}

public record PointerType : IrType
{
    public IrType? Pointee { get; }

    public PointerType(IrType? pointee = null)
    {
        Pointee = pointee;
    }

    public override string ToString() => "ptr";
}

public record PtrType : IrType
{
    public IrType? Pointee { get; }

    public PtrType(IrType? pointee = null)
    {
        Pointee = pointee;
    }

    public override string ToString() => "ptr";
}

public record LiteralStructType : IrType
{
    public Dictionary<string, IrType> Fields { get; set; }

    public LiteralStructType(Dictionary<string, IrType> fields)
    {
        Fields = fields;
    }

    public override string ToString() => "{ " + string.Join(", ", Fields.Select(f => f.ToString())) + " }";
}

public record IdentifiedStructType : LiteralStructType
{
    public string Name { get; }

    public IdentifiedStructType(string name) : base(new Dictionary<string, IrType>())
    {
        Name = name;
    }

    public override string ToString() => $"%{Name}";
}

public record ArrayType : IrType
{
    public IrType ElementType { get; }
    public int Size { get; }

    public ArrayType(IrType elementType, int size)
    {
        ElementType = elementType;
        Size = size;
    }

    public override string ToString() => $"[{Size} x {ElementType}]";
}
