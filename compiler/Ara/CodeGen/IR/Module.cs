using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

public class Module
{
    readonly List<Function> functions = new();

    public Function AppendFunction(string name, FunctionType type)
    {
        var func = new Function(name, type);
        functions.Add(func);
        return func;
    }

    public string Emit()
    {
        var sb = new StringBuilder();
        functions.ForEach(f => f.Emit(sb));
        return sb.ToString().Trim();
    }
}
