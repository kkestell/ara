#region

using System.Text;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR;

public class Module
{
    private readonly List<Struct> _structs = new();
    private readonly List<ExternalFunctionDeclaration> _externalFunctionDeclarations = new();
    private readonly List<FunctionDeclaration> _functionDeclarations = new();
    private readonly List<Function> _functionDefinitions = new();

    public Struct AddStruct(string name, IEnumerable<StructField> fields)
    {
        var s = new Struct(name, fields);
        _structs.Add(s);
        return s;
    }
    
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
        
        foreach (var s in _structs)
        {
            sb.AppendLine($"%{s.Name} = type {{ {string.Join(", ", s.Fields.Select(x => x.Type.ToIr()))} }}");
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
