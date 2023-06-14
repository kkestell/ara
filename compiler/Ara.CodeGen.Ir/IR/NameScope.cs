namespace Ara.CodeGen.Ir.IR
{
    public class NameScope
    {
        private readonly Dictionary<string, int> _nameCount = new();
        private readonly HashSet<string> _names = new();
        private uint _counter;

        public IEnumerable<string> Names => _names;

        public string Register(string? name)
        {
            if (name is null)
                return _counter++.ToString();
            
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty", name);
            
            lock (_names)
            {
                var uniqueName = Dedupe(name);
                _names.Add(uniqueName);
                return uniqueName;
            }
        }

        private string Dedupe(string name)
        {
            if (!_names.Contains(name))
            {
                _nameCount[name] = 1;
                return name;
            }

            var newName = $"{name}.{_nameCount[name]}";
            _nameCount[name]++;
            return newName;
        }
    }
}
