namespace Ara.CodeGen.IR;

public class NameScope
{
    readonly HashSet<string> names = new();
    int counter;

    public NameScope()
    {
    }

    NameScope(HashSet<string> names, int counter)
    {
        this.names = names;
        this.counter = counter;
    }

    public NameScope Clone()
    {
        return new NameScope(new HashSet<string>(names), counter);
    }

    public string Register(string? name = null)
    {
        name = Dedupe(name);
        names.Add(name);
        return name;
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
