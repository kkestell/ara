// using System.Text;
// using Ara.CodeGen.Ir.IR.Values;
//
// namespace Ara.CodeGen.Ir.IR;
//
// public class Block
// {
//     // readonly InstructionList instructions = new();
//     // readonly Block? parent;
//     // readonly NameScope scope;
//
//     public Block(Function function)
//     {
//         // Function = function;
//         // scope = new NameScope();
//         // Label = new Label(this, "entry");
//         // AddInstruction(Label);
//     }
//     
//     // public Block(Function function, Block parent, string name)
//     // {
//     //     this.parent = parent;
//     //     Function = function;
//     //     // scope = new NameScope(parent.scope);
//     //     Label = new Label(this, Function.Scope.Register(name));
//     //     AddInstruction(Label);
//     // }
//     //
//     // public Function Function { get; }
//     //
//     // public Label Label { get; }
//
//     // public int InstructionCount => instructions.Count;
//     //
//     // public IEnumerable<Value> Instructions => instructions.Values;
//     //
//     // public void PositionBefore(Value instruction)
//     // {
//     //     instructions.PositionBefore(instruction);
//     // }
//     //
//     // public void PositionAfter(Value instruction)
//     // {
//     //     instructions.PositionAfter(instruction);
//     // }
//     //
//     // public void PositionAtStart()
//     // {
//     //     instructions.PositionAtStart();
//     // }
//     //
//     // public void PositionAtEnd()
//     // {
//     //     instructions.PositionAtEnd();
//     // }
//
//     // public string RegisterName(string name)
//     // {
//     //     return Function.Scope.Register(name);
//     // }
//
//     // public Block AddChild(string? name = null)
//     // {
//     //     return Function.AddBlock(this, name);
//     // }
//     
//     // public IrBuilder IrBuilder()
//     // {
//     //     return new IrBuilder(this);
//     // }
//     
//     // public T AddInstruction<T>(T i) where T : Value
//     // {
//     //     instructions.Insert(i);
//     //     return i;
//     // }
//     
//     // public void Emit(StringBuilder sb)
//     // {
//     //     foreach (var inst in instructions)
//     //     {
//     //         if (inst is NamedValue)
//     //         {
//     //             var sb2 = new StringBuilder();
//     //             inst.Emit(sb2);
//     //             Console.WriteLine($"Emitting named value {inst.GetType()} ({sb2.ToString().Trim()})");
//     //         }
//     //         else if (inst is Value)
//     //         {
//     //             Console.WriteLine($"Emitting value {inst.GetType()}");
//     //         }
//     //         inst.Emit(sb);
//     //     }
//     // }
//     //
//     // public T FindNamedValue<T>(string valueName) where T: Value
//     // {
//     //     return (T)FindNamedValue(valueName);
//     // }
//     //
//     // public Value FindNamedValue(string valueName)
//     // {
//     //     foreach (var inst in instructions)
//     //     {
//     //         if (inst is not NamedValue v)
//     //             continue;
//     //         
//     //         if (v.Name == valueName)
//     //             return v;
//     //     }
//     //
//     //     if (parent is not null)
//     //         return parent.FindNamedValue(valueName);
//     //
//     //     var arg = Function.Argument(valueName);
//     //
//     //     if (arg is not null)
//     //         return arg;
//     //
//     //     throw new Exception($"Named value {valueName} not found");
//     // }
// }