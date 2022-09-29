using System.Text;
using Ara.CodeGen.Errors;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class GetElementPtr : Instruction
{
    readonly Value array;
    readonly Value index;

    public override IrType Type => IrType.Void;

    public GetElementPtr(Block block, Value array, Value index, string? name = null) : base(block, name)
    {
        if (array.Type.GetType() != typeof(PointerType))
            throw new ArgumentException();

        this.array = array;
        this.index = index;
    }
    
    public override void Emit(StringBuilder sb)
    {
        if (array.Type is not PointerType p)
            throw new CodeGenException($"Value is not a pointer");

        if (p.Type is not ArrayType a)
            throw new CodeGenException("Pointee is not an array");

        sb.AppendLine($"{Resolve()} = getelementptr inbounds [0 x {a.Type.ToIr()}], ptr {array.Resolve()}, i32 0, i32 {index.Resolve()}");
    }
}