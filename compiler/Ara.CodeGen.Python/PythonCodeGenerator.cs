using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Statements;
using System.Text;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Nodes.Statements.Abstract;

namespace Ara.CodeGen.Python
{
    public class PythonCodeGenerator : ICodeGenerator
    {
        public string Generate(SourceFile root)
        {
            StringBuilder pythonCode = new StringBuilder();

            foreach (var f in root.FunctionDefinitions.Nodes)
            {
                GenerateFunction(pythonCode, f);
            }

            return pythonCode.ToString();
        }

        private void GenerateFunction(StringBuilder pythonCode, FunctionDefinition functionDefinition)
        {
            if (functionDefinition.Name == "main")
            {
                pythonCode.AppendLine("if __name__ == \"__main__\":");
            }
            else
            {
                pythonCode.Append($"def {functionDefinition.Name}(");
                pythonCode.Append(string.Join(", ", functionDefinition.Parameters.Nodes.Select(p => p.Name)));
                pythonCode.AppendLine("):");
            }
    
            GenerateBlock(pythonCode, functionDefinition.Block, 1);
        }

        private void GenerateBlock(StringBuilder pythonCode, Block block, int indentLevel)
        {
            foreach (var statement in block.Statements.Nodes)
            {
                GenerateStatement(pythonCode, statement, indentLevel);
            }
        }

        private void GenerateStatement(StringBuilder pythonCode, Statement statement, int indentLevel)
        {
            string indent = new string('\t', indentLevel);

            switch (statement)
            {
                case Block b:
                    GenerateBlock(pythonCode, b, indentLevel);
                    break;
                case Assignment a:
                    pythonCode.AppendLine($"{indent}{a.Name} = {EmitExpression(a.Expression)}");
                    break;
                case If i:
                    pythonCode.AppendLine($"{indent}if {EmitExpression(i.Predicate)}:");
                    GenerateStatement(pythonCode, i.Then, indentLevel + 1);
                    break;
                case IfElse ie:
                    pythonCode.AppendLine($"{indent}if {EmitExpression(ie.Predicate)}:");
                    GenerateStatement(pythonCode, ie.Then, indentLevel + 1);
                    pythonCode.AppendLine($"{indent}else:");
                    GenerateStatement(pythonCode, ie.Else, indentLevel + 1);
                    break;
                case Return r:
                    pythonCode.AppendLine($"{indent}return {EmitExpression(r.Expression)}");
                    break;
                case VariableDeclaration v:
                    pythonCode.AppendLine($"{indent}{v.Name} = {EmitExpression(v.Expression)}");
                    break;
                case For f:
                    pythonCode.AppendLine($"{indent}for {f.Counter} in range({EmitExpression(f.Start)}, {EmitExpression(f.End)}):");
                    GenerateBlock(pythonCode, f.Block, indentLevel + 1);
                    break;
                case Call c:
                    pythonCode.AppendLine($"{indent}{EmitExpression(c)}");
                    break;
            }
        }

        private string EmitExpression(Expression expression)
        {
            return expression switch
            {
                ArrayIndex        e => EmitArrayIndex(e),
                BinaryExpression  e => EmitBinaryExpression(e),
                Call              e => EmitCall(e),
                IntegerValue      e => MakeInteger(e),
                FloatValue        e => MakeFloat(e),
                BooleanValue      e => MakeBoolean(e),
                VariableReference e => EmitVariableReference(e),
            
                _ => throw new Exception($"Unsupported expression type {expression.GetType()}.")
            };
        }
        
        private string EmitArrayIndex(ArrayIndex expression)
        {
            return $"{EmitExpression(expression.VariableReference)}[{EmitExpression(expression.Index)}]";
        }

        private string EmitBinaryExpression(BinaryExpression expression)
        {
            string op = expression.Op switch
            {
                BinaryOperator.Add => "+",
                BinaryOperator.Subtract => "-",
                BinaryOperator.Multiply => "*",
                BinaryOperator.Divide => "/",
                BinaryOperator.Equality => "==",
                BinaryOperator.Inequality => "!=",
                _ => throw new Exception($"Unsupported binary operator {expression.Op}")
            };
    
            return $"{EmitExpression(expression.Left)} {op} {EmitExpression(expression.Right)}";
        }

        private string EmitCall(Call expression)
        {
            var arguments = string.Join(", ", expression.Arguments.Nodes.Select(x => EmitExpression(x.Expression)));
            return $"{expression.Name}({arguments})";
        }

        private string MakeInteger(IntegerValue expression)
        {
            return expression.Value.ToString();
        }

        private string MakeFloat(FloatValue expression)
        {
            return expression.Value.ToString();
        }

        private string MakeBoolean(BooleanValue expression)
        {
            return expression.Value ? "True" : "False";
        }

        private string EmitVariableReference(VariableReference expression)
        {
            return expression.Name;
        }

        
        /*
        private string GenerateExpression(Expression expression)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    return $"({GenerateExpression(binaryExpression.Left)} {binaryExpression.Op} {GenerateExpression(binaryExpression.Right)})";
                case IntegerValue integerValue:
                    return integerValue.Value.ToString();
                case FloatValue floatValue:
                    return floatValue.Value.ToString();
                case BooleanValue booleanValue:
                    return booleanValue.Value ? "True" : "False";
                case VariableReference variableReference:
                    return variableReference.Name;
                case Call functionCall:
                    var args = string.Join(", ", functionCall.Arguments.Nodes.Select(x => GenerateExpression(x.Expression)));
                    return $"{functionCall.Name}({args})";
                default:
                    throw new NotImplementedException($"Unsupported expression type {expression.GetType().Name}.");
            }
        }
        */
    }
}
