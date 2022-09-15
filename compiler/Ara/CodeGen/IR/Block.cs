using System.Text;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

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


    public string RegisterName(string name)
    {
        return scope.Register(name);
    }

    public Block AddChildBlock(Label name)
    {
        var newBlock = new Block(Function, name, this);
        Function.AddBlock(newBlock);
        return newBlock;
    }
    
    public IrBuilder Builder()
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

    public T NamedValue<T>(string valueName) where T: Value
    {
        return (T)NamedValue(valueName);
    }

    public Value NamedValue(string valueName)
    {
        foreach (var inst in instructions)
        {
            if (inst is not NamedValue v)
                continue;
            
            if (v.Name == valueName)
                return v;
        }

        if (parent is not null)
            return parent.NamedValue(valueName);
        
        throw new Exception($"Named value {valueName} not found");
    }
}