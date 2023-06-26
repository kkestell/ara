// using Ara.Ast.Nodes;
// using Ara.Ast.Nodes.Expressions;
// using Ara.Ast.Nodes.Statements;
// using System.Text;
// using Ara.Ast.Nodes.Expressions.Abstract;
// using Ara.Ast.Nodes.Expressions.Values;
// using Ara.Ast.Nodes.Statements.Abstract;
//
// namespace Ara.CodeGen.C;
//
// public class CCodeGenerator : ICodeGenerator
// {
//     private readonly Dictionary<string, FunctionType> _functionTypes = new();
//
//     public string Generate(SourceFile root)
//     {
//         StringBuilder cCode = new StringBuilder();
//
//         if (root.ExternalFunctionDeclarations is not null)
//         {
//             foreach (var f in root.ExternalFunctionDeclarations.Nodes)
//             {
//                 _functionTypes.Add(f.Name, FunctionType.FromExternalDeclaration(f));
//             }
//         }
//
//         foreach (var f in root.FunctionDefinitions.Nodes)
//         {
//             _functionTypes.Add(f.Name, FunctionType.FromDefinition(f));
//         }
//
//         if (root.ExternalFunctionDeclarations is not null)
//         {
//             foreach (var f in root.ExternalFunctionDeclarations.Nodes)
//             {
//                 EmitExternalFunctionDeclaration(cCode, f);
//             }
//         }
//
//         foreach (var f in root.FunctionDefinitions.Nodes)
//         {
//             EmitFunction(cCode, f);
//         }
//
//         return cCode.ToString();
//     }
//
//     private void EmitExternalFunctionDeclaration(StringBuilder cCode, ExternalFunctionDeclaration externalFunctionDeclaration)
//     {
//         cCode.Append($"extern {externalFunctionDeclaration.Type.ToCType()} {externalFunctionDeclaration.Name}(");
//         cCode.Append(string.Join(", ", externalFunctionDeclaration.Parameters.Nodes.Select(x => $"{x.Type.ToCType()} {x.Name}")));
//         cCode.AppendLine(");");
//     }
//
//     private void EmitFunction(StringBuilder cCode, FunctionDefinition functionDefinition)
//     {
//         var type = _functionTypes[functionDefinition.Name];
//         cCode.Append($"{type.ToCType()} {functionDefinition.Name}(");
//         cCode.Append(string.Join(", ", functionDefinition.Parameters.Nodes.Select(x => $"{x.Type.ToCType()} {x.Name}")));
//         cCode.AppendLine(") {");
//         EmitBlock(cCode, functionDefinition.Block);
//         if (functionDefinition.Type is VoidType)
//         {
//             cCode.AppendLine("return;");
//         }
//         cCode.AppendLine("}");
//     }
//
//     private void EmitBlock(StringBuilder cCode, Block block)
//     {
//         foreach (var statement in block.Statements.Nodes)
//         {
//             EmitStatement(cCode, statement);
//         }
//     }
//
//     private void EmitStatement(StringBuilder cCode, Statement s)
//     {
//         switch (s)
//         {
//             case Assignment a:
//                 EmitAssignment(cCode, a);
//                 break;
//             case ArrayAssignment a:
//                 EmitArrayAssignment(cCode, a);
//                 break;
//             case Call c:
//                 EmitCall(cCode, c);
//                 break;
//             case Block b:
//                 EmitBlock(cCode, b);
//                 break;
//             case For f:
//                 EmitFor(cCode, f);
//                 break;
//             case If i:
//                 EmitIf(cCode, i);
//                 break;
//             case IfElse i:
//                 EmitIfElse(cCode, i);
//                 break;
//             case Return r:
//                 EmitReturn(cCode, r);
//                 break;
//             case VariableDeclaration v:
//                 EmitVariableDeclaration(cCode, v);
//                 break;
//         }
//     }
//
//     private void EmitAssignment(StringBuilder cCode, Assignment a)
//     {
//         cCode.Append($"{a.Name} = ");
//         EmitExpression(cCode, a.Expression);
//         cCode.AppendLine(";");
//     }
//
//     private void EmitArrayAssignment(StringBuilder cCode, ArrayAssignment a)
//     {
//         cCode.Append($"{a.Name}[");
//         EmitExpression(cCode, a.Index);
//         cCode.Append("] = ");
//         EmitExpression(cCode, a.Expression);
//         cCode.AppendLine(";");
//     }
//
//     private void EmitCall(StringBuilder cCode, Call c)
//     {
//         cCode.Append($"{c.Name}(");
//         for (int i = 0; i < c.Arguments.Count; i++)
//         {
//             EmitExpression(cCode, c.Arguments[i]);
//             if (i < c.Arguments.Count - 1)
//                 cCode.Append(", ");
//         }
//         cCode.AppendLine(");");
//     }
//
//     private void EmitFor(StringBuilder cCode, For f)
//     {
//         cCode.AppendLine("for (");
//         EmitVariableDeclaration(cCode, f.VariableDeclaration);
//         EmitExpression(cCode, f.Condition);
//         cCode.Append("; ");
//         EmitAssignment(cCode, f.Assignment);
//         cCode.AppendLine(") {");
//         EmitBlock(cCode, f.Block);
//         cCode.AppendLine("}");
//     }
//
//     private void EmitIf(StringBuilder cCode, If i)
//     {
//         cCode.Append("if (");
//         EmitExpression(cCode, i.Condition);
//         cCode.AppendLine(") {");
//         EmitBlock(cCode, i.Block);
//         cCode.AppendLine("}");
//     }
//
//     private void EmitIfElse(StringBuilder cCode, IfElse i)
//     {
//         cCode.Append("if (");
//         EmitExpression(cCode, i.Condition);
//         cCode.AppendLine(") {");
//         EmitBlock(cCode, i.IfBlock);
//         cCode.AppendLine("} else {");
//         EmitBlock(cCode, i.ElseBlock);
//         cCode.AppendLine("}");
//     }
//
//     private void EmitReturn(StringBuilder cCode, Return r)
//     {
//         cCode.Append("return ");
//         EmitExpression(cCode, r.Expression);
//         cCode.AppendLine(";");
//     }
//
//     private void EmitVariableDeclaration(StringBuilder cCode, VariableDeclaration v)
//     {
//         cCode.AppendLine($"{v.Type.ToCType()} {v.Name} = ");
//         EmitExpression(cCode, v.Expression);
//         cCode.AppendLine(";");
//     }
//
//     private void EmitExpression(StringBuilder cCode, Expression e)
//     {
//         switch (e)
//         {
//             case IntegerValue iv:
//                 cCode.Append(iv.Value);
//                 break;
//             case FloatValue fv:
//                 cCode.Append(fv.Value);
//                 break;
//             case BooleanValue bv:
//                 cCode.Append(bv.Value ? "true" : "false");
//                 break;
//             case Call c:
//                 EmitCall(cCode, c);
//                 break;
//         }
//     }
// }
