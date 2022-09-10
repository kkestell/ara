using System.Text;
using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

namespace Ara.CodeGen.IR;

public class Block
{
    readonly List<Value> instructions = new();

    public T AddInstruction<T>(T i) where T : Instruction
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
}