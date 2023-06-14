#region

using System.Text;
using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Ir.IR;

public class Function
{
    private readonly string _name;
    private readonly FunctionType _type;
    private readonly Dictionary<string, ArgumentValue> _arguments = new();
    private readonly InstructionList _instructions = new();
    private readonly NameScope _scope = new();

    public Function(string name, FunctionType type)
    {
        _name = name;
        _type = type;
        
        foreach (var p in type.Parameters)
        {
            _arguments.Add(p.Name, new ArgumentValue(this, p.Type, p.Name));
        }

        AddInstruction(new Label(this, "entry"));
    }

    
    public string NextName()
    {
        return _scope.Register(null);
    }
    
    public Value? Argument(string argName)
    {
        return !_arguments.ContainsKey(argName) ? null : _arguments[argName];
    }

    public void Emit(StringBuilder sb)
    {
        var p = string.Join(", ", _type.Parameters.Select(x => x.ToIr()));
        sb.AppendLine($"define {_type.ReturnType.ToIr()} @{_name} ({p}) {{");
        foreach (var inst in _instructions)
        {
            inst.Emit(sb);
        }
        sb.AppendLine("}");
    }
    
    public int InstructionCount => _instructions.Count;

    public IEnumerable<Value> Instructions => _instructions.Values;
    
    public void PositionBefore(Value instruction)
    {
        _instructions.PositionBefore(instruction);
    }
    
    public void PositionAfter(Value instruction)
    {
        _instructions.PositionAfter(instruction);
    }
    
    public void PositionAtStart()
    {
        _instructions.PositionAtStart();
    }
    
    public void PositionAtEnd()
    {
        _instructions.PositionAtEnd();
    }
    
    public string? RegisterName(string name)
    {
        return _scope.Register(name);
    }

    public T AddInstruction<T>(T i) where T : Value
    {
        _instructions.Insert(i);
        return i;
    }

    public T FindNamedValue<T>(string valueName) where T: Value
    {
        return (T)FindNamedValue(valueName);
    }
    
    public IrBuilder IrBuilder()
    {
        return new IrBuilder(this);
    }
    
    public Value FindNamedValue(string name)
    {
        foreach (var inst in _instructions)
        {
            if (inst is not NamedValue v)
                continue;

            if (v.Name == name)
                return v;
        }

        var arg = Argument(name);

        if (arg is not null)
            return arg;

        throw new Exception($"Named value {name} not found");
    }
}
