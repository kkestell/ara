using System.Text;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

public class Module
{
    readonly List<Function> functions = new();
    readonly List<FunctionDeclaration> functionDeclarations = new();

    public Function AddFunction(string name, FunctionType type)
    {
        var func = new Function(name, type);
        functions.Add(func);
        return func;
    }

    public void DeclareFunction(FunctionDeclaration decl)
    {
        functionDeclarations.Add(decl);
    }

    public string Emit()
    {
        var sb = new StringBuilder();
        functions.ForEach(f => f.Emit(sb));

        foreach (var decl in functionDeclarations)
        {
            sb.AppendLine($"declare {decl.ReturnType.ToIr()} @{decl.Name}({string.Join(", ", decl.ParameterTypes.Select(x => $"{x.ToIr()} noundef"))})");
        }
        
        return sb.ToString().Trim();
    }
}
