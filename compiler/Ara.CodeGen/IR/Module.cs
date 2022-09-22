using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

public class Module
{
    readonly List<Function> functions = new();

    public Function AddFunction(string name, FunctionType type)
    {
        var func = new Function(name, type);
        functions.Add(func);
        return func;
    }

    public string Emit()
    {
        var sb = new StringBuilder();
        functions.ForEach(f => f.Emit(sb));
        sb.AppendLine("declare ptr @GC_malloc(i64 noundef)");
        return sb.ToString().Trim();
    }
}
