// using Ara.Parsing;
// using Ara.Parsing.Nodes;
// using Ara.Parsing.Types;
//
// namespace Ara.Semantics;
//
// public class SymbolTableVisitor
// {
//     private readonly UnknownType _unknownType = new();
//     private readonly Stack<SymbolTable> _scopes = new();
//     protected SymbolTable CurrentScope => _scopes.Peek();
//
//     public void Visit(Node node)
//     {
//         switch (node)
//         {
//             case CompilationUnit compilationUnit:
//                 VisitCompilationUnit(compilationUnit);
//                 break;
//             case ExternFunctionDefinition externFunctionDefinition:
//                 VisitExternFunctionDefinition(externFunctionDefinition);
//                 break;
//             case FunctionDefinition functionDefinition:
//                 VisitFunctionDefinition(functionDefinition);
//                 break;
//             case Parameter parameter:
//                 VisitParameter(parameter);
//                 break;
//             case StructDefinition structDefinition:
//                 VisitStructDefinition(structDefinition);
//                 break;
//             case StructField structField:
//                 VisitStructField(structField);
//                 break;
//             case Block block:
//                 VisitBlock(block);
//                 break;
//             case VariableDeclaration variableDeclaration:
//                 VisitVariableDeclaration(variableDeclaration);
//                 break;
//             case For @for:
//                 VisitFor(@for);
//                 break;
//             default:
//                 foreach (var child in node.Children)
//                     Visit(child);
//                 break;
//         }
//     }
//         
//     public void VisitCompilationUnit(CompilationUnit node)
//     {
//         _scopes.Push(node.SymbolTable);
//         
//         foreach (var child in node.Children)
//             Visit(child);
//         
//         _scopes.Pop();
//     }
//     
//     public void VisitExternFunctionDefinition(ExternFunctionDefinition node)
//     {
//         CurrentScope.Add(new Symbol(node.Name, _unknownType));
//
//         node.Scope = CurrentScope.CreateChild($"Function: {node.Name}");
//         _scopes.Push(node.Scope);
//
//         foreach (var param in node.Parameters)
//             Visit(param);
//         
//         _scopes.Pop();
//     }
//
//     public void VisitFunctionDefinition(FunctionDefinition node)
//     {
//         CurrentScope.Add(new Symbol(node.Name, _unknownType));
//
//         node.Scope = CurrentScope.CreateChild($"Function: {node.Name}");
//         _scopes.Push(node.Scope);
//
//         foreach (var param in node.Parameters)
//             Visit(param);
//         
//         foreach (var statement in node.Body.Statements)
//             Visit(statement);
//
//         _scopes.Pop();
//     }
//
//     public void VisitParameter(Parameter node)
//     {
//         CurrentScope.Add(new Symbol(node.Name, _unknownType));
//     }
//     
//     public void VisitStructDefinition(StructDefinition node)
//     {
//         CurrentScope.Add(new Symbol(node.Name, _unknownType));
//
//         node.Scope = CurrentScope.CreateChild($"Struct: {node.Name}");
//         _scopes.Push(node.Scope);
//         
//         foreach (var child in node.Children)
//             Visit(child);
//         
//         _scopes.Pop();
//     }
//
//     public void VisitStructField(StructField node)
//     {
//         CurrentScope.Add(new Symbol(node.Name, _unknownType));
//     }
//
//     public void VisitBlock(Block node)
//     {
//         node.Scope = CurrentScope.CreateChild("Block");
//         _scopes.Push(node.Scope);
//         
//         foreach (var child in node.Children)
//             Visit(child);
//         
//         _scopes.Pop();
//     }
//
//     public void VisitVariableDeclaration(VariableDeclaration node)
//     {
//         CurrentScope.Add(new Symbol(node.Name, _unknownType));
//     }
//
//     public void VisitFor(For node)
//     {
//         node.Scope = CurrentScope.CreateChild("For");
//         _scopes.Push(node.Scope);
//         
//         Visit(node.Initializer);
//         Visit(node.Body);
//         
//         _scopes.Pop();
//     }
// }
