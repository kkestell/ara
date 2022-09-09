using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

/// <summary>
/// https://llvm.org/docs/LangRef.html#module-structure
/// </summary>
public class Module
{
    readonly List<Function> functions = new();

    public Function AppendFunction(string name, FunctionType type)
    {
        var func = new Function(this, name, type);
        functions.Add(func);
        return func;
    }

    public string Emit()
    {
        var sb = new StringBuilder();
        foreach (var function in functions)
        {
            function.Emit(sb);
        }

        return sb.ToString();
    }
}
