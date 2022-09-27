using System.Collections;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class InstructionList : IEnumerable<Value>
{
    readonly List<Value> instructions = new();
    
    public void Insert(Value instruction)
    {
        instructions.Insert(Pointer, instruction);
        Pointer++;
    }

    public IEnumerable<Value> Instructions => instructions.AsEnumerable();

    public int Pointer { get; private set; }

    public int Count => instructions.Count;

    public void PositionBefore(Value instruction)
    {
        Pointer = instructions.IndexOf(instruction);
    }

    public void PositionAfter(Value instruction)
    {
        Pointer = instructions.IndexOf(instruction) + 1;
    }

    public void PositionAtStart()
    {
        Pointer = 0;
    }

    public void PositionAtEnd()
    {
        Pointer = instructions.Count;
    }

    public IEnumerator<Value> GetEnumerator()
    {
        return instructions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)instructions).GetEnumerator();
    }
}