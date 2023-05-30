#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR;

public class Module
{
    private readonly List<ExternalFunctionDeclaration> _externalFunctionDeclarations = new();
    private readonly List<FunctionDeclaration> _functionDeclarations = new();
    private readonly List<Function> _functionDefinitions = new();

    public Function AddFunction(string name, FunctionType type)
    {
        var func = new Function(name, type);
        _functionDefinitions.Add(func);
        return func;
    }

    public void DeclareFunction(FunctionDeclaration decl)
    {
        _functionDeclarations.Add(decl);
    }

    public void DeclareExternalFunction(ExternalFunctionDeclaration decl)
    {
        _externalFunctionDeclarations.Add(decl);
    }

    public string Emit()
    {
        var sb = new StringBuilder();

        foreach (var decl in _externalFunctionDeclarations)
        {
            sb.AppendLine($"declare {decl.ReturnType.ToIr()} @{decl.Name}({string.Join(", ", decl.ParameterTypes.Select(x => x.ToIr()))})");
        }

        foreach (var func in _functionDefinitions)
        {
            func.Emit(sb);
        }

        foreach (var decl in _functionDeclarations)
        {
            sb.AppendLine($"declare {decl.ReturnType.ToIr()} @{decl.Name}({string.Join(", ", decl.ParameterTypes.Select(x => x.ToIr()))})");
        }
        
        return sb.ToString().Trim();
    }
}
