using System.Text;
using Ara.CodeGen.Types;

namespace Ara.CodeGen.Values;

public abstract class Value
{
    public abstract IrType Type { get; }
    
    public abstract string Resolve();
    
    public abstract void Emit(StringBuilder sb);
}