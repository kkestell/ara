using Ara.IR.Types;

namespace Ara.IR;

public class ExternFunction : IFunction
{
    public string Name { get; }
    public IrType ReturnType { get; }
    public List<IrType> ArgTypes { get; }

    public ExternFunction(string name, IrType returnType, List<IrType> argTypes)
    {
        Name = name;
        ReturnType = returnType;
        ArgTypes = argTypes;
    }

    public override string ToString()
    {
        var argStrings = ArgTypes.Select(argType => argType.ToString());
        var argList = string.Join(", ", argStrings);
        return $"declare {ReturnType} @{Name}({argList})";
    }
}