using System.Text;
using Ara.CodeGen.IR.Types.Abstract;

namespace Ara.CodeGen.IR.Values.Abstract;

public abstract class Value
{
    public abstract IrType Type { get; }
    public abstract string Resolve();
    public abstract void Emit(StringBuilder sb);
}