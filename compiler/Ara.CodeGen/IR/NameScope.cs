namespace Ara.CodeGen.IR;

public class NameScope
{
    readonly HashSet<string> names = new();
    int counter;

    public string Register(string? name = null)
    {
        var uniqueName = Dedupe(name);
        names.Add(uniqueName);
        return uniqueName;
    }

    string NextTemporaryName() => $"{counter++}";
    
    string Dedupe(string? name = null)
    {
        if (string.IsNullOrEmpty(name))
            return NextTemporaryName();
        
        if (!names.Contains(name))
            return name;

        var i = 0;
        while (true)
        {
            var newName = $"{name}.{i}";
            if (!names.Contains(newName))
                return newName;
            i++;
        }
    }
}
