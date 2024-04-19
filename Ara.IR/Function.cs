using Ara.IR.Types;

namespace Ara.IR;

public class Function : IFunction
{
    public string Name { get; }
    public IrType ReturnType { get; }
    public List<IrType> ArgTypes { get; }
    public List<BasicBlock> BasicBlocks { get; } = new ();

    public Function(string name, IrType returnType, List<IrType> argTypes)
    {
        Name = name;
        ReturnType = returnType;
        ArgTypes = argTypes;
    }

    public void AddBasicBlock(BasicBlock block)
    {
        BasicBlocks.Add(block);
    }

    public List<Value> Args => ArgTypes.Select((argType, i) => new Value(argType, $"%{i}")).ToList();

    public override string ToString()
    {
        var argStrings = ArgTypes.Select((argType, i) => $"{argType} %{i}");
        var argList = string.Join(", ", argStrings);
        var header = $"define {ReturnType} @{Name}({argList}) {{";
        var blocks = string.Join("\n", BasicBlocks.Select(block => block.ToString()));
        return $"{header}\n{blocks}\n}}";
    }
}
