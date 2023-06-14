#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR.Values;

public abstract class Value
{
    public abstract IrType Type { get; }
    
    public abstract string Resolve();
    
    public abstract void Emit(StringBuilder sb);
}