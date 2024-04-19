using System.Diagnostics;
using Ara.Parsing;
using Ara.Parsing.Nodes;
using Ara.Parsing.Types;
using Type = Ara.Parsing.Types.Type;

namespace Ara.Semantics;

public class TypeVisitor
{
    public void Visit(Node node)
    {
        switch (node)
        {
            case CompilationUnit compilationUnit:
                Visit(compilationUnit);
                break;
            
            case ExternFunctionDefinition externFunctionDefinition:
                Visit(externFunctionDefinition);
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
            case PointerTypeRef pointerTypeRef:
                Visit(pointerTypeRef);
                break;
            case ArrayTypeRef arrayTypeRef:
                Visit(arrayTypeRef);
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
                Visit(variableDeclaration);
                break;
            case Assignment assignment:
                Visit(assignment);
                break;
            case Return returnStatement:
                Visit(returnStatement);
                break;
            case If ifStatement:
                Visit(ifStatement);
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
            case CastExpression castExpression:
                Visit(castExpression);
                break;
            case DereferenceExpression dereferenceExpression:
                Visit(dereferenceExpression);
                break;
            case AddressOfExpression addressOfExpression:
                Visit(addressOfExpression);
                break;
        }
    }

    public void Visit(CompilationUnit node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }
    
    public void Visit(ExternFunctionDefinition node)
    {
        foreach (var param in node.Parameters)
            Visit(param);
        var returnType = ResolveType(node.TypeRef);
        var paramTypes = node.Parameters.Select(param => param.Type).ToList().AsReadOnly();
        var functionType = new FunctionType(returnType, paramTypes);
        node.Type = functionType;
    }

    public void Visit(FunctionDefinition node)
    {
        foreach (var param in node.Parameters)
            Visit(param);
        
        var returnType = ResolveType(node.TypeRef);
        var paramTypes = node.Parameters.Select(param => param.Type).ToList().AsReadOnly();
        var functionType = new FunctionType(returnType, paramTypes);
        node.Type = functionType;

        foreach (var statement in node.Body.Statements)
            Visit(statement);
    }

    public void Visit(Parameter node)
    {
        node.Type = ResolveType(node.TypeRef);
    }
    
    public void Visit(BasicTypeRef node)
    {
        var type = ResolveType(node);
        node.Type = type;
    }
    
    public void Visit(PointerTypeRef node)
    {
        var type = ResolveType(node);
        node.Type = type;
    }

    public void Visit(ArrayTypeRef node)
    {
        var type = ResolveType(node);
        node.Type = type;
    }
    
    public void Visit(StructDefinition node)
    {
        // Create a stub symbol to allow recursive references
        var structType = new StructType(node.Name);
        node.Type = structType;

        foreach (var child in node.Children)
            Visit(child);

        // Update the struct type with the fields
        var fieldTypes = node.Fields.ToDictionary(field => field.Name, field => field.Type!);
        structType.Fields = fieldTypes;
    }

    public void Visit(StructField node)
    {
        node.Type = ResolveType(node.TypeRef);
    }

    public void Visit(Block node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    public void Visit(VariableDeclaration node)
    {
        node.Type = ResolveType(node.TypeRef);

        if (node.Expression is null) 
            return;
        
        Visit(node.Expression);
        node.Expression = HandleImplicitCast(node.Expression, node.Type, node.Location);

        if (node.Type != node.Expression.Type)
        {
            throw new SemanticException($"Type mismatch in assignment. Cannot assign '{node.Expression.Type}' to '{node.Type}'.", node.Location, null);
        }
    }

    public void Visit(Assignment node)
    {
        Visit(node.Left);
        
        Visit(node.Right);
        node.Right = HandleImplicitCast(node.Right, node.Left.Type, node.Location);

        if (node.Left.Type != node.Right.Type)
        {
            throw new SemanticException($"Type mismatch in assignment. Cannot assign '{node.Right.Type}' to '{node.Left.Type}'.", node.Location, null);
        }
    }

    public void Visit(Return node)
    {
        var functionDefinition = node.FindParent<FunctionDefinition>();
        
        if (functionDefinition is null)
        {
            throw new SemanticException("Return statement must be inside a function.", node.Location, null);
        }
        
        var currentFunctionReturnType = functionDefinition.Type.ReturnType;
        
        if (currentFunctionReturnType is VoidType)
        {
            if (node.Expression != null)
            {
                throw new SemanticException("Cannot return a value from a function with a void return type.", node.Location, null);
            }
        }
        else if (node.Expression is null)
        {
            throw new SemanticException("Function must return a value.", node.Location, null);
        }
        else
        {
            Visit(node.Expression);
            
            node.Expression = HandleImplicitCast(node.Expression, currentFunctionReturnType, node.Location);
            
            if (node.Expression.Type != currentFunctionReturnType)
            {
                throw new SemanticException($"Return type mismatch. Expected '{currentFunctionReturnType}' but got '{node.Expression.Type}'.", node.Location, null);
            }
        }
    }

    public void Visit(If node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    public void Visit(While node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    public void Visit(For node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    public void Visit(Break node)
    {
    }

    public void Visit(Continue node)
    {
    }

    public void Visit(ExpressionStatement node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    public void Visit(Identifier node)
    {
        var declaration = node.FindDeclaration(node.Value);

        node.Type = declaration switch
        {
            null => throw new SemanticException($"Identifier '{node.Value}' not found.", node.Location, null),
            VariableDeclaration variableDeclaration => variableDeclaration.Type,
            Parameter parameter => parameter.Type,
            _ => throw new SemanticException($"Identifier '{node.Value}' is not a variable or parameter.",
                node.Location, null)
        };
    }

    public void Visit(UnaryExpression node)
    {
        foreach (var child in node.Children)
            Visit(child);
        
        node.Type = node.Operand.Type;
    }

    public void Visit(BinaryExpression node)
    {
        Visit(node.Left);
        Visit(node.Right);

        if (node.Left.Type != node.Right.Type)
        {
            if (IsSafeImplicitCast(node.Left.Type, node.Right.Type))
            {
                node.Left = HandleImplicitCast(node.Left, node.Right.Type, node.Location);
            }
            else if (IsSafeImplicitCast(node.Right.Type, node.Left.Type))
            {
                node.Right = HandleImplicitCast(node.Right, node.Left.Type, node.Location);
            }
            else
            {
                throw new SemanticException($"Type mismatch in binary expression between '{node.Left.Type}' and '{node.Right.Type}'.", node.Location, null);
            }
        }

        node.Type = node.Left.Type;
    }

    public void Visit(MemberAccessExpression node)
    {
        Visit(node.Left);

        if (node.Right is not Identifier memberIdentifier)
            throw new SemanticException("Right side of member access expression must be an identifier.", node.Location, null);

        switch (node.Left.Type)
        {
            case StructType structType:
            {
                if (!structType.Fields.TryGetValue(memberIdentifier.Value, out var memberType))
                    throw new SemanticException($"Struct '{structType.Name}' does not have a member '{memberIdentifier.Value}'.", node.Location, null);

                node.Type = memberType;
                break;
            }
            case PointerType { ElementType: StructType structType2 }:
            {
                if (!structType2.Fields.TryGetValue(memberIdentifier.Value, out var memberType))
                    throw new SemanticException($"Struct '{structType2.Name}' does not have a member '{memberIdentifier.Value}'.", node.Location, null);

                node.Type = memberType;
                break;
            }
            default:
                throw new SemanticException($"Cannot access member '{memberIdentifier.Value}' on non-struct type '{node.Left.Type}'.", node.Location, null);
        }
    }

    public void Visit(CallExpression node)
    {
        if (node.Callee is not Identifier identifier)
            throw new SemanticException("Callee of call expression must be an identifier.", node.Location, null);

        var definition = node.FindDeclaration(identifier.Value);

        var functionType = definition switch
        {
            FunctionDefinition functionDefinition => functionDefinition.Type,
            ExternFunctionDefinition externFunctionDefinition => externFunctionDefinition.Type,
            _ => throw new SemanticException($"'{identifier.Value}' is not a function.", node.Location, null)
        };

        if (functionType is null)
            throw new SemanticException($"Function '{identifier.Value}' has no type.", node.Location, null);
        
        if (node.Arguments.Count != functionType.ParameterTypes.Count)
            throw new SemanticException($"Function '{identifier.Value}' expects {functionType.ParameterTypes.Count} arguments but got {node.Arguments.Count}.", node.Location, null);

        for (var i = 0; i < node.Arguments.Count; i++)
        {
            var arg = node.Arguments[i];
            Visit(arg);

            var expectedType = functionType.ParameterTypes[i];
            arg.Expression = HandleImplicitCast(arg.Expression, expectedType, arg.Location);

            if (arg.Expression.Type != expectedType)
            {
                throw new SemanticException($"Argument type mismatch in function call. Expected '{expectedType}' but got '{arg.Expression.Type}'.", arg.Location, null);
            }
        }

        node.Type = functionType.ReturnType;
    }

    public void Visit(Argument node)
    {
        Visit(node.Expression);
        node.Type = node.Expression.Type;
    }

    public void Visit(IndexAccessExpression node)
    {
        if (node.Left is not Identifier identifier)
            throw new SemanticException("Left side of index access expression must be an identifier.", node.Location, null);

        var definition = node.FindDeclaration(identifier.Value);
        
        var type = definition switch
        {
            null => throw new SemanticException($"Identifier '{identifier.Value}' not found.", node.Location, null),
            Parameter parameter => parameter.Type,
            VariableDeclaration variableDeclaration => variableDeclaration.Type,
            _ => throw new SemanticException($"Identifier '{identifier.Value}' is not a variable.", node.Location, null)
        };
        
        if (type is not ArrayType arrayType)
            throw new SemanticException($"Cannot index non-array type '{type}'.", node.Location, null);
        
        Visit(node.Right);

        if (node.Right.Type is not IntType)
            throw new SemanticException($"Index must be an integer type, but got '{node.Right.Type}'.", node.Location, null);

        if (node.Right is IntegerLiteral integerLiteral)
        {
            var intValue = integerLiteral.Value;
            
            if (intValue < 0 || intValue >= arrayType.Size)
                throw new SemanticException($"Index out of bounds. Expected index to be in range [0, {arrayType.Size}), but got {intValue}.", node.Location, null);
        }
        
        node.Type = arrayType.ElementType;
    }

    public void Visit(ArrayInitializationExpression node)
    {
        if (node.Elements.Count == 0)
            throw new SemanticException("Cannot create an array with no elements.", node.Location, null);
        
        for (var i = 0; i < node.Elements.Count; i++)
        {
            var element = node.Elements[i];
            Visit(element);
            node.Elements[i] = HandleImplicitCast(element, node.Elements[0].Type, element.Location);
        }
        
        var elementType = node.Elements[0].Type;
        Debug.Assert(elementType is not null);
        
        if (node.Elements.Any(element => element.Type != elementType))
            throw new SemanticException($"All elements of an array must be of the same type, but got '{elementType}' and '{node.Elements[0].Type}'.", node.Location, null);
        
        node.Type = new ArrayType(elementType, node.Elements.Count);
    }
    
    public void Visit(CastExpression node)
    {
        foreach (var child in node.Children)
            Visit(child);
        
        node.Type = ResolveType(node.TypeRef);
    }
    
    public void Visit(DereferenceExpression node)
    {
        Visit(node.Operand); // Ensure the operand expression is fully visited

        if (node.Operand.Type is PointerType pointerType)
        {
            // The type of the dereference expression is the element type of the pointer
            node.Type = pointerType.ElementType;
        }
        else
        {
            throw new SemanticException($"Cannot dereference non-pointer type '{node.Operand.Type}'.", node.Location, null);
        }
    }

    public void Visit(AddressOfExpression node)
    {
        Visit(node.Operand); // Ensure the operand expression is fully visited

        // Ensure the operand is an lvalue (e.g., a variable or a dereference of a pointer)
        if (node.Operand is Identifier || node.Operand is DereferenceExpression)
        {
            // The type of the address-of expression is a pointer to the operand's type
            node.Type = new PointerType(node.Operand.Type);
        }
        else
        {
            throw new SemanticException($"Cannot take the address of a non-lvalue expression.", node.Location, null);
        }
    }


    private Type ResolveType(TypeRef typeRef)
    {
        switch (typeRef)
        {
            case ArrayTypeRef arrayTypeRef:
                var elementType = ResolveType(arrayTypeRef.ElementType);
                return new ArrayType(elementType, arrayTypeRef.Size);
            case PointerTypeRef pointerTypeRef:
                var pointerElementType = ResolveType(pointerTypeRef.ElementType);
                return new PointerType(pointerElementType);
            case BasicTypeRef basicTypeRef:
            {
                switch (basicTypeRef.Name)
                {
                    case "void":
                        return new VoidType();
                    case "i8":
                        return new IntType(8);
                    case "i16":
                        return new IntType(16);
                    case "i32":
                        return new IntType(32);
                    case "i64":
                        return new IntType(64);                    
                    case "u8":
                        return new IntType(8, false);
                    case "u16":
                        return new IntType(16, false);
                    case "u32":
                        return new IntType(32, false);
                    case "u64":
                        return new IntType(64, false);
                    case "f32":
                        return new FloatType(32);
                    case "f64":
                        return new FloatType(64);
                    case "bool":
                        return new BoolType();
                    case "string":
                        return new StringType();
                    default:
                    {
                        if (typeRef.TryFindDeclaration(basicTypeRef.Name, out var symbol))
                        {
                            var symbolType = symbol switch
                            {
                                StructDefinition structDefinition => structDefinition.Type,
                                _ => throw new Exception($"Unknown symbol type: {symbol}")
                            };
                            return symbolType;
                        }

                        break;
                    }
                }
            }
            break;
        }

        throw new Exception($"Unknown type: {typeRef}");
    }
    
    // Helpers
    
    private Expression HandleImplicitCast(Expression expression, Type targetType, Location location)
    {
        if (expression.Type != targetType && IsSafeImplicitCast(expression.Type, targetType))
        {
            // void pointer to any pointer
            if (expression.Type is PointerType && targetType is PointerType)
            {
                expression.Type = targetType;
                return expression;
            }
            
            switch (expression)
            {
                case IntegerLiteral intLiteral when targetType is IntType:
                    intLiteral.Type = targetType; // Directly change the type
                    return intLiteral;
                case FloatLiteral floatLiteral when targetType is FloatType:
                    floatLiteral.Type = targetType; // Directly change the type
                    return floatLiteral;
                default:
                {
                    var typeRef = new BasicTypeRef(targetType.ToString(), location)
                    {
                        Type = targetType
                    };
                    return new CastExpression(typeRef, expression, location)
                    {
                        Type = targetType
                    };
                }
            }
        }
        
        return expression;
    }
    
    private static bool IsSafeImplicitCast(Type? fromType, Type? toType)
    {
        return fromType switch
        {
            // Void pointer to any pointer
            PointerType when toType is PointerType => true,
            // Integer to larger or same-sized integer
            IntType fromIntType when toType is IntType intToIntType => fromIntType.SizeBits <= intToIntType.SizeBits,
            // Integer to any floating-point type
            IntType when toType is FloatType => true,
            // Floating-point to larger or same-sized floating-point
            FloatType fromFloatType when toType is FloatType floatToFloatType => fromFloatType.SizeBits <= floatToFloatType.SizeBits,
            _ => false
        };
    }
}
