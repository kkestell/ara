using System.Text;
using Ara.Parsing;
using Ara.Parsing.Nodes;
using Ara.Parsing.Types;
using Type = System.Type;

namespace Ara.CodeGen.C;
    
using System;
using System.Text;

public class CCodeGenerator
{
    private readonly StringBuilder _sb = new();
    private int indent;
    
    public string Generate(Node node)
    {
        _sb.AppendLine("#include <iostream>");
        _sb.AppendLine("#include <memory>");
        _sb.AppendLine("#include <string>");
        Visit(node);
        return _sb.ToString();
    }
    
    public void Visit(Node node, bool inline = false)
    {
        switch (node)
        {
            case CompilationUnit compilationUnit:
                Visit(compilationUnit);
                break;
            case FunctionDefinition functionDefinition:
                Visit(functionDefinition);
                break;
            case Parameter parameter:
                Visit(parameter);
                break;
            case BasicTypeRef basicTypeRef:
                Visit(basicTypeRef);
                break;
            case ArrayTypeRef arrayTypeRef:
                Visit(arrayTypeRef);
                break;
            case ClassDefinition classDefinition:
                Visit(classDefinition);
                break;
            case MethodDefinition methodDefinition:
                Visit(methodDefinition);
                break;
            case ClassField classField:
                Visit(classField);
                break;
            case StructDefinition structDefinition:
                Visit(structDefinition);
                break;
            case StructField structField:
                Visit(structField);
                break;
            case Block block:
                Visit(block);
                break;
            case VariableDeclaration variableDeclaration:
                Visit(variableDeclaration, inline);
                break;
            case Assignment assignment:
                Visit(assignment, inline);
                break;
            case Return returnStatement:
                Visit(returnStatement);
                break;
            case IfVar ifVarStatement:
                Visit(ifVarStatement);
                break;
            case If ifStatement:
                Visit(ifStatement);
                break;
            case Elif elifStatement:
                Visit(elifStatement);
                break;
            case Else elseStatement:
                Visit(elseStatement);
                break;
            case While whileStatement:
                Visit(whileStatement);
                break;
            case For forStatement:
                Visit(forStatement);
                break;
            case Break breakStatement:
                Visit(breakStatement);
                break;
            case Continue continueStatement:
                Visit(continueStatement);
                break;
            case ExpressionStatement expressionStatement:
                Visit(expressionStatement);
                break;
            case Identifier identifier:
                Visit(identifier);
                break;
            case IntegerLiteral integerLiteral:
                Visit(integerLiteral);
                break;
            case FloatLiteral floatLiteral:
                Visit(floatLiteral);
                break;
            case StringLiteral stringLiteral:
                Visit(stringLiteral);
                break;
            case BooleanLiteral booleanLiteral:
                Visit(booleanLiteral);
                break;
            case UnaryExpression unaryExpression:
                Visit(unaryExpression);
                break;
            case BinaryExpression binaryExpression:
                Visit(binaryExpression);
                break;
            case MemberAccessExpression memberAccessExpression:
                Visit(memberAccessExpression);
                break;
            case CallExpression callExpression:
                Visit(callExpression);
                break;
            case Argument argument:
                Visit(argument);
                break;
            case IndexAccessExpression indexAccessExpression:
                Visit(indexAccessExpression);
                break;
            case ArrayInitializationExpression arrayInitializationExpression:
                Visit(arrayInitializationExpression);
                break;
            case NewExpression newExpression:
                Visit(newExpression);
                break;
        }
    }
        
    public void Visit(CompilationUnit node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    public void Visit(FunctionDefinition node)
    {
        _sb.Append($"{MakeType(node.Type.ReturnType)} {node.Name}(");
        var parameters = node.Parameters.Select(param => $"{MakeType(param.Type)} {param.Name}");
        _sb.Append(string.Join(", ", parameters));
        _sb.Append(")\n");
        _sb.Append("{\n");
        foreach (var statement in node.Body.Statements)
            Visit(statement);
        _sb.Append("}\n");
    }

    public void Visit(ClassDefinition node)
    {
        _sb.Append($"class {node.Name}\n");
        _sb.Append("{\n");
        _sb.Append("public:\n");
        foreach (var child in node.Children)
        {
            Visit(child);
        }
        _sb.Append("};\n");
    }

    public void Visit(MethodDefinition node)
    {
        _sb.Append($"{MakeType(node.Type.ReturnType)} {node.Name}(");
        var parameters = node.Parameters.Select(param => $"{MakeType(param.Type)} {param.Name}");
        _sb.Append(string.Join(", ", parameters));
        _sb.Append(")\n");
        _sb.Append("{\n");
        foreach (var statement in node.Body.Statements)
            Visit(statement);
        _sb.Append("}\n");
    }

    public void Visit(ClassField node)
    {
        _sb.Append($"{MakeType(node.Type)} {node.Name};\n");
    }

    public void Visit(StructDefinition node)
    {
        _sb.Append($"struct {node.Name}\n");
        _sb.Append("{\n");
        foreach (var child in node.Children)
            Visit(child);
        _sb.Append("};\n");
    }

    public void Visit(StructField node)
    {
        _sb.Append($"{MakeType(node.Type)} {node.Name};\n");
    }

    public void Visit(Block node)
    {
        _sb.Append("{\n");
        foreach (var child in node.Children)
            Visit(child);
        _sb.Append("}\n");
    }
    
    public void Visit(VariableDeclaration node, bool inline = false)
    {
        _sb.Append($"{MakeType(node.Type)} {node.Name}");
        if (node.Expression is not null)
        {
            _sb.Append(" = ");
            Visit(node.Expression);
        }
        if (!inline)
        {
            _sb.Append(";\n");
        }
    }

    public void Visit(Assignment node, bool inline = false)
    {
        Visit(node.Identifier);
        _sb.Append(" = ");
        Visit(node.Expression);
        if (!inline)
        {
            _sb.Append(";\n");
        }
    }

    public void Visit(Return node)
    {
        _sb.Append("return ");
        if (node.Expression.Type is not VoidType)
        {
            Visit(node.Expression);
        }
        _sb.Append(";\n");
    }

    public void Visit(IfVar node)
    {
        _sb.Append($"if (auto {node.Name} = ");
        Visit(node.Expression);
        _sb.Append(")\n");
        Visit(node.Then);
    }

    public void Visit(If node)
    {
        _sb.Append("if (");
        Visit(node.Condition);
        _sb.Append(")\n");
        Visit(node.Then);
    }

    public void Visit(Elif node)
    {
        _sb.Append("else if (");
        Visit(node.Condition);
        _sb.Append(")\n");
        Visit(node.Then);
    }

    public void Visit(Else node)
    {
        _sb.Append("else\n");
        Visit(node.Then);
    }

    public void Visit(While node)
    {
        _sb.Append("while (");
        Visit(node.Condition);
        _sb.Append(")\n");
        Visit(node.Then);
    }

    public void Visit(For node)
    {
        _sb.Append("for (");
        Visit(node.Initializer, inline: true);
        _sb.Append("; ");
        Visit(node.Condition);
        _sb.Append("; ");
        Visit(node.Increment, inline: true);
        _sb.Append(")\n");
        Visit(node.Body);
    }

    public void Visit(Break node)
    {
    }

    public void Visit(Continue node)
    {
    }

    public void Visit(ExpressionStatement node)
    {
        Visit(node.Expression);
        _sb.Append(";\n");
    }

    public void Visit(Identifier node)
    {
        _sb.Append(node.Value);
    }

    public void Visit(IntegerLiteral node)
    {
        _sb.Append(node.Value);
    }

    public void Visit(FloatLiteral node)
    {
        _sb.Append(node.Value);
    }

    public void Visit(StringLiteral node)
    {
        _sb.Append($"\"{node.Value}\"");
    }

    public void Visit(BooleanLiteral node)
    {
        _sb.Append(node.Value ? "true" : "false");
    }

    public void Visit(UnaryExpression node)
    {
        _sb.Append(MakeOperator(node.Operator));
        Visit(node.Right);
    }

    public void Visit(BinaryExpression node)
    {
        Visit(node.Left);
        _sb.Append($" {MakeOperator(node.Operator)} ");
        Visit(node.Right);
    }

    public void Visit(MemberAccessExpression node)
    {
        Visit(node.Left);
        if (node.Left.Type is PointerType)
        {
            _sb.Append("->");
        }
        else
        {
            _sb.Append(".");
        }
        Visit(node.Right);
    }

    public void Visit(CallExpression node)
    {
        Visit(node.Callee);
        _sb.Append("(");
        foreach (var argument in node.Arguments)
        {
            Visit(argument);
            if (node.Arguments.IndexOf(argument) != node.Arguments.Count - 1)
            {
                _sb.Append(", ");
            }
        }
        _sb.Append(")");
    }

    public void Visit(Argument node)
    {
        Visit(node.Expression);
    }

    public void Visit(IndexAccessExpression node)
    {
        Visit(node.Left);
        _sb.Append("[");
        Visit(node.Right);
        _sb.Append("]");
    }

    public void Visit(ArrayInitializationExpression node)
    {
        _sb.Append("{");
        foreach (var element in node.Elements)
        {
            Visit(element);
            if (node.Elements.IndexOf(element) != node.Elements.Count - 1)
            {
                _sb.Append(", ");
            }
        }
        _sb.Append("}");
    }

    public void Visit(NewExpression node)
    {
        if (node.Type is not SharedReferenceType sharedReferenceType)
            throw new Exception("Expected shared reference type");
        
        if (sharedReferenceType.ElementType is not ClassType classType)
            throw new Exception("Expected class type");
        
        _sb.Append("std::make_shared<");
        _sb.Append(classType.Name);
        _sb.Append(">(");
        foreach (var argument in node.Arguments)
        {
            Visit(argument);
            if (node.Arguments.IndexOf(argument) != node.Arguments.Count - 1)
            {
                _sb.Append(", ");
            }
        }
        _sb.Append(")");
    }
    
    private string MakeType(Ara.Parsing.Types.Type type)
    {
        if (type is VoidType)
            return "void";
        if (type is IntType)
            return "int";
        if (type is FloatType)
            return "float";
        if (type is BoolType)
            return "bool";
        if (type is StringType)
            return "std::string";
        if (type is ArrayType arrayType)
            return $"{MakeType(arrayType.ElementType)}[{string.Join(", ", arrayType.Sizes)}]";
        if (type is PointerType pointerType)
            return $"{MakeType(pointerType.ElementType)}*";
        if (type is SharedReferenceType sharedReferenceType)
            return $"std::shared_ptr<{MakeType(sharedReferenceType.ElementType)}>";
        if (type is ReferenceType weakReferenceType)
            return $"std::weak_ptr<{MakeType(weakReferenceType.ElementType)}>";
        if (type is GenericType genericType)
            return $"{genericType.Name}<{string.Join(", ", genericType.TypeArguments.Select(MakeType))}>";
        if (type is ClassType classType)
            return classType.Name;
        if (type is StructType structType)
            return structType.Name;
        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }

    private string MakeOperator(TokenType op)
    {
        return op switch {
            TokenType.Plus => "+",
            TokenType.Minus => "-",
            TokenType.Star => "*",
            TokenType.Slash => "/",
            TokenType.Modulo => "%",
            TokenType.BangEqual => "!=",
            TokenType.EqualEqual => "==",
            TokenType.Greater => ">",
            TokenType.GreaterEqual => ">=",
            TokenType.Less => "<",
            TokenType.LessEqual => "<=",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }
}
