// using System.Text;
// using Ara.Parsing.Types;
// using Type = Ara.Parsing.Types.Type;
//
// namespace Ara.Parsing;
//
// public class Symbol
// {
//     public string Name { get; set; }
//     public Type Type { get; set; }
//     
//     public Symbol(string name, Type? type = null)
//     {
//         Name = name;
//         Type = type ?? new UnknownType();
//     }
// }
//
// public class SymbolTable
// {
//     private readonly string _name;
//     private readonly SymbolTable? _parent;
//     private readonly List<SymbolTable> _children = new();
//     private readonly Dictionary<string, Symbol> _symbols = new();
//     
//     public SymbolTable(string name, SymbolTable? parent = null)
//     {
//         _name = name;
//         _parent = parent;
//     }
//     
//     public SymbolTable CreateChild(string name)
//     {
//         var child = new SymbolTable(name, this);
//         _children.Add(child);
//         return child;
//     }
//     
//     public void Add(Symbol symbol)
//     {
//         if (TryGet(symbol.Name, out _))
//             throw new Exception($"Symbol '{symbol.Name}' already defined.");
//         
//         _symbols.TryAdd(symbol.Name, symbol);
//     }
//
//     public Symbol Get(string name)
//     {
//         if (_symbols.TryGetValue(name, out var symbol))
//         {
//             return symbol;
//         }
//         
//         if (_parent is not null)
//         {
//             return _parent.Get(name);
//         }
//         
//         throw new Exception($"Symbol '{name}' not found.");
//     }
//     
//     public bool TryGet(string name, out Symbol? symbol)
//     {
//         if (_symbols.TryGetValue(name, out symbol))
//         {
//             return true;
//         }
//         
//         if (_parent is not null)
//         {
//             return _parent.TryGet(name, out symbol);
//         }
//         
//         return false;
//     }
//     
//     public override string ToString()
//     {
//         return GetString();
//     }
//     
//     private string GetString(int indentLevel = 0)
//     {
//         var builder = new StringBuilder();
//
//         var headingIndent = new string(' ', indentLevel * 4);
//         var symbolsIndent = new string(' ', (indentLevel + 1) * 4);
//         var symbolColumnWidth = 40 - (4 * indentLevel);
//
//         builder.AppendLine($"{headingIndent}{_name}");
//
//         if (_symbols.Any())
//         {
//             foreach (var entry in _symbols)
//             {
//                 builder.AppendLine($"{symbolsIndent}{entry.Key.PadRight(symbolColumnWidth)} {entry.Value.Type}");
//             }
//         }
//         else
//         {
//             builder.AppendLine($"{symbolsIndent}{"-".PadRight(symbolColumnWidth)} -");
//         }
//
//         builder.AppendLine();
//
//         foreach (var child in _children)
//         {
//             builder.Append(child.GetString(indentLevel + 1));
//         }
//
//         return builder.ToString();
//     }
//
// }