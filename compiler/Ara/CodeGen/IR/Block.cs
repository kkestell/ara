using System.Text;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class Block
{
    readonly string name;
    readonly List<Value> instructions = new();
    readonly Block? parent;

    public Block(string name, Function function, Block? parent = null)
    {
        this.name = name;
        this.parent = parent;
        Function = function;

        if (parent is not null)
        {
            Scope = parent.Scope.Clone();
        }
        else
        {
            Scope = new NameScope();
        }
    }
    
    public Function Function { get; }

    public NameScope Scope { get; }

    public Block AddChildBlock(string childName)
    {
        var newBlock = new Block(childName, Function, this);
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
        sb.AppendLine($"{name}:");
        foreach (var inst in instructions)
        {
            inst.Emit(sb);
        }
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