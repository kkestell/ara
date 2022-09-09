using Ara.CodeGen.IR.Types.Abstract;

namespace Ara.CodeGen.IR.Types;

/// <summary>
/// https://llvm.org/docs/LangRef.html#floating-point-types
/// </summary>
public class FloatType : IrType
{
    public override string ToIr() => "float";

    public override bool Equals(object? obj)
    {
        return obj is FloatType;
    }
}