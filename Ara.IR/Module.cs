using System.Text;
using Ara.IR.Types;

namespace Ara.IR;

public class Module
{
    public List<ExternFunction> ExternFunctions { get; } = [];
    public List<Function> Functions { get; } = [];
    public Dictionary<string, IdentifiedStructType> Structs { get; } = new();

    public void AddExternFunction(ExternFunction func)
    {
        ExternFunctions.Add(func);
    }
    
    public void AddFunction(Function func)
    {
        Functions.Add(func);
    }

    public void AddStruct(string name, IdentifiedStructType structType)
    {
        Structs.Add(name, structType);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        foreach (var (structName, structType) in Structs)
        {
            sb.AppendLine($"%{structName} = type {{");
            var fieldTypes = structType.Fields.Values.ToList();
            for(var i = 0; i < fieldTypes.Count; i++)
            {
                var fieldType = fieldTypes[i];
                sb.Append($"  {fieldType}");
                if (i < structType.Fields.Count - 1)
                {
                    sb.Append(",");
                }
                sb.AppendLine();
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }
        
        foreach (var func in Functions)
        {
            sb.AppendLine(func.ToString());
        }

        sb.AppendLine("declare noalias ptr @malloc(i64 noundef)");
        sb.AppendLine("declare void @free(ptr noundef)");
        sb.AppendLine("declare noalias i32 @putchar(i32 noundef)");
        sb.AppendLine("declare noalias i32 @getchar()");
        
        return sb.ToString();
    }
}
