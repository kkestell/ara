using System.Text;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class Block
{
    readonly List<Value> instructions = new();

    public T AddInstruction<T>(T i) where T : Value
    {
        instructions.Add(i);
        return i;
    }

    public NameScope Scope { get; } = new();

    public void Emit(StringBuilder sb)
    {
        foreach (var inst in instructions)
        {
            inst.Emit(sb);
        }
    }

    public Value NamedValue(string name)
    {
        foreach (var inst in instructions)
        {
            if (inst is not NamedValue v)
                continue;
            
            if (v.Name == name)
                return v;
        }
        
        throw new Exception($"Named value {name} not found");
    }
}