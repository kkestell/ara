#region

using System.Text;
using Ara.CodeGen.Errors;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR.Values.Instructions;

public class GetElementPtr : Instruction
{
    private readonly Value _array;
    private readonly Value _index;

    public override IrType Type { get; }

    public GetElementPtr(Function function, Value array, Value index, string? name = null) : base(function, name)
    {
        Type = array.Type;

        _array = array;
        _index = index;
    }
    
    public override void Emit(StringBuilder sb)
    {
        if (_array.Type is not PointerType pointerType)
            throw new CodeGenException("Not a pointer!");
    //     if (_array.Type is not ArrayType a)
    //         throw new CodeGenException("Pointee is not an array");

        if (pointerType.Type is not ArrayType a)
            throw new CodeGenException("Pointee is not an array");
    
        sb.AppendLine($"{Resolve()} = getelementptr [{a.Size} x {a.Type.ToIr()}], ptr {_array.Resolve()}, i32 0, i32 {_index.Resolve()}");
    }
}