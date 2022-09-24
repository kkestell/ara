using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values.Instructions;

public class Load : Instruction
{
    readonly NamedValue pointer;

    public Load(Block block, NamedValue pointer, string? name = null) : base(block, name)
    {
        if (pointer.Type.GetType() != typeof(PointerType))
            throw new ArgumentException("Argument is not a pointer");
        
        this.pointer = pointer;
    }
    
    public override IrType Type
    {
        get
        {
            if (pointer.Type is PointerType ptrType)
            {
                return ptrType.Type;
            }

            throw new NotSupportedException("Not a pointer!");
        }
    }
    
    public override void Emit(StringBuilder sb)
    {
        if (pointer.Type is PointerType ptrType)
        {
            sb.Append($"{Resolve()} = load {ptrType.Type.ToIr()}, ptr {pointer.Resolve()}\n");
        }
        else
        {
            throw new NotSupportedException("Not a pointer!");
        }
    }
}