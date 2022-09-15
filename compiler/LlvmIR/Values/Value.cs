using System.Text;
using LlvmIR.Types;

namespace LlvmIR.Values;

public abstract class Value
{
    public abstract IrType Type { get; }
    
    public abstract string Resolve();
    
    public abstract void Emit(StringBuilder sb);
}