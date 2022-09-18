using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR.Values;

public abstract class Value
{
    public abstract IrType Type { get; }
    
    public abstract string Resolve();
    
    public abstract void Emit(StringBuilder sb);
}