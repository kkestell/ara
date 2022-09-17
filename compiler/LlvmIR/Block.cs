using System.Text;
using LlvmIR.Values;

namespace LlvmIR;

public class Block
{
    readonly List<Value> instructions = new();
    readonly Block? parent;
    readonly NameScope scope;

    public Block(Function function)
    {
        Function = function;
        scope = new NameScope();
        AddInstruction(new Label(this, "entry"));
    }
    
    public Block(Function function, Label name, Block parent)
    {
        this.parent = parent;
        Function = function;
        scope = parent.scope.Clone();
        AddInstruction(name);
    }
    
    public Function Function { get; }


    public string RegisterName(string? name = null)
    {
        return scope.Register(name);
    }

    public Block AddChild(Label name)
    {
        return Function.AddBlock(name, this);
    }
    
    public IrBuilder IrBuilder()
    {
        return new IrBuilder(this);
    }
    
    public T AddInstruction<T>(T i) where T : Value
    {
        instructions.Add(i);
        return i;
    }
    
    public void Emit(StringBuilder sb)
    {
        foreach (var inst in instructions)
        {
            inst.Emit(sb);
        }
    }

    public T FindNamedValue<T>(string valueName) where T: Value
    {
        return (T)FindNamedValue(valueName);
    }

    public Value FindNamedValue(string valueName)
    {
        foreach (var inst in instructions)
        {
            if (inst is not NamedValue v)
                continue;
            
            if (v.Name == valueName)
                return v;
        }

        if (parent is not null)
            return parent.FindNamedValue(valueName);

        var arg = Function.Argument(valueName);

        if (arg is not null)
            return arg;

        throw new Exception($"Named value {valueName} not found");
    }
}