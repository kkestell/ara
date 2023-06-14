#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values.Instructions;

public class Load : Instruction
{
    private readonly Value _pointer;

    public Load(Function function, Value pointer, string? name = null) : base(function, name)
    {
        if (pointer.Type.GetType() != typeof(PointerType))
            throw new ArgumentException("Argument is not a pointer");
        
        _pointer = pointer;
    }
    
    public override IrType Type
    {
        get
        {
            if (_pointer.Type is not PointerType ptrType) 
                throw new NotSupportedException("Not a pointer!");
            
            if (ptrType.Type is ArrayType arrayType)
                return arrayType.Type;

            return ptrType.Type;

        }
    }
    
    public override void Emit(StringBuilder sb)
    {
        if (_pointer.Type is PointerType ptrType)
        {
            sb.Append($"{Resolve()} = load {ptrType.Type.ToIr()}, ptr {_pointer.Resolve()}\n");
        }
        else
        {
            throw new NotSupportedException("Not a pointer!");
        }
    }
}