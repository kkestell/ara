using Ara.CodeGen.IR.Types.Abstract;

namespace Ara.CodeGen.IR.Types;

/// <summary>
/// https://llvm.org/docs/LangRef.html#void-type
/// </summary>
public class VoidType : IrType
{
    public override string ToIr() => "void";
    
    public override bool Equals(object? obj)
    {
        return obj is VoidType;
    }
}