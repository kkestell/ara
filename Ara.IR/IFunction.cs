using Ara.IR.Types;

namespace Ara.IR;

public interface IFunction
{
    string Name { get; }
    IrType ReturnType { get; }
    List<IrType> ArgTypes { get; }
}