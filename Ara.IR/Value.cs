using System.Globalization;
using Ara.IR.Types;

namespace Ara.IR;

public class Value
{
    public IrType IRType { get; set; }
    public string? Name { get; }
    public object? Constant { get; }

    public Value(IrType irType, string? name = null, object? constant = null)
    {
        IRType = irType;
        Name = name;
        Constant = constant;
    }

    public override string ToString()
    {
        if (Name is null)
        {
            if (IRType is VoidType)
            {
                return "void";
            }

            return $"{IRType} {FormattedConstant}";
        }

        return $"{IRType} {Name}";
    }

    public string ConstantOrName => Name ?? FormattedConstant;

    public string FormattedConstant
    {
        get
        {
            switch (Constant)
            {
                case null:
                    return IRType switch
                    {
                        VoidType _ => "void",
                        BooleanType _ => "0",
                        IntegerType _ => "0",
                        FloatType _ => "0.0",
                        PointerType _ => "null",
                        PtrType ptrType => ptrType.Pointee is null ? "null" : throw new NotImplementedException(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                case float f:
                {
                    // Blah!
                    byte[] bytes = BitConverter.GetBytes((double)f);
                    Array.Reverse(bytes);
                    string hexString = BitConverter.ToString(bytes).Replace("-", "");
                    return "0x" + hexString;
                }
                case bool b:
                    return b ? "1" : "0";
                default:
                    return Constant.ToString()!;
            }
        }
    }
}