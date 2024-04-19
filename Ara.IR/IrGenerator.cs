using System.Diagnostics;
using Ara.IR.Instructions;
using Ara.IR.Types;
using Ara.Parsing;
using Ara.Parsing.Nodes;

namespace Ara.IR;

public class IrGenerator
{
    private readonly Module _module;
    private readonly IrBuilder _irBuilder;
    private readonly Dictionary<string, int> _labelCounter = new();
    private BasicBlock? _currentLoopEndBlock;
    private BasicBlock? _currentLoopContinueBlock;

    public IrGenerator()
    {
        _module = new Module();
        _irBuilder = new IrBuilder(_module);
    }
    
    public Module Generate(CompilationUnit root)
    {
        foreach (var externFunctionDefinition in root.Definitions.OfType<ExternFunctionDefinition>())
        {
            GenerateExternFunctionDefinition(externFunctionDefinition);
        }
        
        foreach (var structDefinition in root.Definitions.OfType<StructDefinition>())
        {
            GenerateStructDefinition(structDefinition);
        }
        
        foreach (var functionDefinition in root.Definitions.OfType<FunctionDefinition>())
        {
            GenerateFunctionDefinition(functionDefinition);
        }
        
        return _module;
    }
    
    // Top-level

    private void GenerateExternFunctionDefinition(ExternFunctionDefinition externFunctionDefinition)
    {
        Debug.Assert(externFunctionDefinition.Type is not null);

        var type = MakeType(externFunctionDefinition.Type.ReturnType);

        var argTypes = externFunctionDefinition.Parameters.Select(parameter =>
        {
            Debug.Assert(parameter.Type is not null);
            var irType = MakeType(parameter.Type);
            
            if (irType is ArrayType)
            {
                irType = new PointerType(irType);
            }
            
            return irType;
        }).ToList();
        
        var externFunction = new ExternFunction(externFunctionDefinition.Name, type, argTypes);

        _module.AddExternFunction(externFunction);
    }
    
    private void GenerateFunctionDefinition(FunctionDefinition functionDefinition)
    {
        Debug.Assert(functionDefinition.Type is not null);

        var returnType = MakeType(functionDefinition.Type.ReturnType);

        var argTypes = functionDefinition.Parameters.Select(parameter =>
        {
            Debug.Assert(parameter.Type is not null);
            var irType = MakeType(parameter.Type);
            if (irType is ArrayType arrayType)
            {
                return new PtrType(arrayType.ElementType);
            }
            return irType;
        }).ToList();

        var function = new Function(functionDefinition.Name, returnType, argTypes);

        _module.AddFunction(function);

        _irBuilder.SetFunction(function);

        var entryBlock = _irBuilder.AppendBasicBlock("entry");
        _irBuilder.SetBlock(entryBlock);

        for (var i = 0; i < functionDefinition.Parameters.Count; i++)
        {
            var param = functionDefinition.Parameters[i];
            var paramType = MakeType(param.Type);
            Value value;

            if (paramType is ArrayType or PointerType)
            {
                // For arrays, directly use the function argument as it is already a pointer
                value = function.Args[i];
            }
            else
            {
                // For non-array types, allocate space and store the argument
                value = _irBuilder.Alloca(paramType);
                _irBuilder.Store(function.Args[i], value);
            }

            _irBuilder.SetValue(param.Name, value);
        }

        foreach (var statement in functionDefinition.Body.Statements)
        {
            _irBuilder.Comment(statement.ToString());
            GenerateStatement(statement);
        }

        if (!IsLastInstructionOfType<Ret>(_irBuilder.CurrentBlock))
        {
            _irBuilder.Ret(new Value(returnType));
        }
    }
    
    private void GenerateStructDefinition(StructDefinition structDefinition)
    {
        // Create a stub type to allow recursive references
        var structType = new IdentifiedStructType(structDefinition.Name);
        _module.AddStruct(structDefinition.Name, structType);

        var fields = new Dictionary<string, IrType>();

        foreach (var field in structDefinition.Fields)
        {
            Debug.Assert(field.Type is not null);
            fields.Add(field.Name, MakeType(field.Type));
        }

        structType.Fields = fields;
    }
    
    // Statements

    private void GenerateStatement(Statement statement)
    {
        switch (statement)
        {
            case Block block:
                GenerateBlock(block);
                break;
            case Return returnStatement:
                GenerateReturn(returnStatement);
                break;
            case VariableDeclaration variableDeclarationStatement:
                GenerateVariableDeclaration(variableDeclarationStatement);
                break;
            case Assignment assignmentStatement:
                GenerateAssignment(assignmentStatement);
                break;
            case If ifStatement:
                GenerateIf(ifStatement);
                break;
            case While whileStatement:
                GenerateWhile(whileStatement);
                break;
            case For forStatement:
                GenerateFor(forStatement);
                break;
            case Break breakStatement:
                GenerateBreak(breakStatement);
                break;
            case Continue continueStatement:
                GenerateContinue(continueStatement);
                break;
            case ExpressionStatement expressionStatement:
                GenerateExpressionStatement(expressionStatement);
                break;
        }
    }

    private void GenerateBlock(Block block)
    {
        foreach (var statement in block.Statements)
        {
            GenerateStatement(statement);
        }
    }

    private void GenerateVariableDeclaration(VariableDeclaration variableDeclarationStatement)
    {
        Debug.Assert(variableDeclarationStatement.Type is not null);

        var type = MakeType(variableDeclarationStatement.Type);
        var variableAlloc = _irBuilder.Alloca(type);

        _irBuilder.SetValue(variableDeclarationStatement.Name, variableAlloc);

        switch (variableDeclarationStatement.Expression)
        {
            case null:
                return;
            case MemberAccessExpression memberAccessExpression:
            {
                var exprValue = GenerateExpression(memberAccessExpression);
                exprValue = LoadIfPointer(exprValue);
                
                _irBuilder.Store(exprValue, variableAlloc);

                break;
            }
            case ArrayInitializationExpression arrayInitializationExpression:
            {
                Debug.Assert(arrayInitializationExpression.Type is not null);
                
                for (var i = 0; i < arrayInitializationExpression.Elements.Count; i++)
                {
                    var elementInitValue = GenerateExpression(arrayInitializationExpression.Elements[i]);
                    var indexValue = new Value(new IntegerType(32, true), constant: i);
                    var elementPtr = _irBuilder.Gep(variableAlloc, new Value(new IntegerType(32, true), constant: 0), indexValue);
                    
                    _irBuilder.Store(elementInitValue, elementPtr);
                }
                
                break;
            }
            default:
            {
                var exprValue = GenerateExpression(variableDeclarationStatement.Expression);
                exprValue = LoadIfPointer(exprValue);
                
                _irBuilder.Store(exprValue, variableAlloc);

                break;
            }
        }
    }
    
    private void GenerateAssignment(Assignment assignmentStatement)
    {
        switch (assignmentStatement.Left)
        {
            case MemberAccessExpression memberAccessExpression:
            {
                var exprValue = GenerateExpression(assignmentStatement.Right);
                exprValue = LoadIfPointer(exprValue);

                var elementPointer = GenerateExpression(memberAccessExpression);

                _irBuilder.Store(exprValue, elementPointer);
    
                break;
            }
            case Identifier identifier:
            {
                var exprValue = GenerateExpression(assignmentStatement.Right);
                exprValue = LoadIfPointer(exprValue);
                
                var name = identifier.Value;
                var varPtr = _irBuilder.GetValue(name);

                _irBuilder.Store(exprValue, varPtr);
                
                break;
            }
            case IndexAccessExpression indexAccessExpression:
            {
                var exprValue = GenerateExpression(assignmentStatement.Right);

                var arrayValue = GenerateExpression(indexAccessExpression.Left);
                var indexValue = GenerateExpression(indexAccessExpression.Right);

                // Check if arrayValue is a pointer to an array or a scalar type
                if (!(arrayValue.IRType is PtrType pointerType && 
                      (pointerType.Pointee is ArrayType || IsScalarType(pointerType.Pointee))))
                {
                    throw new InvalidOperationException("Left side of index access expression must be a pointer to an array or a scalar type.");
                }

                // Generate a GEP instruction. 
                // For scalar types, the index access directly computes the offset.
                // For array types, the first index (0) accesses the first element of the array, 
                // and the second index (indexValue) computes the offset within the array.
                Value elementPtr;
                if (IsScalarType(pointerType.Pointee))
                {
                    elementPtr = _irBuilder.Gep(arrayValue, indexValue);
                }
                else
                {
                    elementPtr = _irBuilder.Gep(arrayValue, new Value(new IntegerType(32, true), constant: 0), indexValue);
                }
                
                _irBuilder.Store(exprValue, elementPtr);
                
                break;
            }
            case DereferenceExpression dereferenceExpression:
            {
                var exprValue = GenerateExpression(assignmentStatement.Right);

                // Generate the pointer expression from the DereferenceExpression
                var pointerValue = GenerateExpression(dereferenceExpression.Operand);

                // Check if the pointerValue is actually a pointer type
                if (pointerValue.IRType is not PtrType && pointerValue.IRType is not PointerType)
                    throw new InvalidOperationException("Dereference expression must have a pointer type");

                if (pointerValue.IRType is PtrType)
                {
                    pointerValue = _irBuilder.Load(pointerValue);
                }
                
                // Directly store 'exprValue' into the location pointed to by 'pointerValue'
                // No need to load from 'pointerValue' before storing
                _irBuilder.Store(exprValue, pointerValue);

                break;
            }
            default:
                throw new ArgumentException($"Left side of assignment must be an identifier, member access, or index access, but was {assignmentStatement.Left}");
        }
    }
    
    public void GenerateReturn(Return returnStatement)
    {
        Debug.Assert(_irBuilder.CurrentFunction is not null);
        
        if (returnStatement.Expression is null)
        {
            _irBuilder.Ret(new Value(_irBuilder.CurrentFunction.ReturnType));
            return;
        }

        var exprValue = GenerateExpression(returnStatement.Expression);
        
        exprValue = LoadIfPointer(exprValue);

        _irBuilder.Ret(exprValue);
    }
    
    private void GenerateIf(If ifStatement)
    {
        Debug.Assert(_irBuilder.CurrentBlock is not null);
        
        var conditionValue = GenerateExpression(ifStatement.Condition);

        var thenBlock = _irBuilder.AppendBasicBlock(UniqueLabel("if_then"));
        var elseBlock = _irBuilder.AppendBasicBlock(UniqueLabel("if_else"));
        var endBlock = _irBuilder.AppendBasicBlock(UniqueLabel("if_end"));

        _irBuilder.Br(conditionValue, thenBlock, elseBlock);

        _irBuilder.SetBlock(thenBlock);
        GenerateStatement(ifStatement.Then);

        if (!IsLastInstructionOfType<Ret>(_irBuilder.CurrentBlock))
            _irBuilder.Br(endBlock);

        _irBuilder.SetBlock(elseBlock);
        if (ifStatement.Else != null)
        {
            if (ifStatement.Else is If nestedIf)
            {
                GenerateIf(nestedIf);
            }
            else
            {
                GenerateStatement(ifStatement.Else);
            }
        }

        if (!IsLastInstructionOfType<Ret>(_irBuilder.CurrentBlock))
        {
            _irBuilder.Br(endBlock);
        }

        _irBuilder.SetBlock(endBlock);
    }
    
    private void GenerateWhile(While whileStatement)
    {
        var prevLoopEndBlock = _currentLoopEndBlock;
        var prevLoopContinueBlock = _currentLoopContinueBlock;

        var conditionBlock = _irBuilder.AppendBasicBlock(UniqueLabel("while_cond"));
        var bodyBlock = _irBuilder.AppendBasicBlock(UniqueLabel("while_body"));
        var endBlock = _irBuilder.AppendBasicBlock(UniqueLabel("while_end"));

        _currentLoopEndBlock = endBlock;
        _currentLoopContinueBlock = conditionBlock;

        _irBuilder.Br(conditionBlock);

        _irBuilder.SetBlock(conditionBlock);
        var conditionValue = GenerateExpression(whileStatement.Condition);
        _irBuilder.Br(conditionValue, bodyBlock, endBlock);

        _irBuilder.SetBlock(bodyBlock);
        GenerateStatement(whileStatement.Then);

        if (!IsLastInstructionOfType<Ret>(_irBuilder.CurrentBlock))
            _irBuilder.Br(conditionBlock);

        _irBuilder.SetBlock(endBlock);

        _currentLoopEndBlock = prevLoopEndBlock;
        _currentLoopContinueBlock = prevLoopContinueBlock;
    }

    private void GenerateFor(For forStatement)
    {
        var initBlock = _irBuilder.AppendBasicBlock(UniqueLabel("for_init"));
        var conditionBlock = _irBuilder.AppendBasicBlock(UniqueLabel("for_cond"));
        var bodyBlock = _irBuilder.AppendBasicBlock(UniqueLabel("for_body"));
        var incrementBlock = _irBuilder.AppendBasicBlock(UniqueLabel("for_incr"));
        var endBlock = _irBuilder.AppendBasicBlock(UniqueLabel("for_end"));

        _irBuilder.Br(initBlock);

        _irBuilder.SetBlock(initBlock);
        GenerateStatement(forStatement.Initializer);
        _irBuilder.Br(conditionBlock);

        _irBuilder.SetBlock(conditionBlock);
        var conditionValue = GenerateExpression(forStatement.Condition);
        _irBuilder.Br(conditionValue, bodyBlock, endBlock);

        _irBuilder.SetBlock(bodyBlock);
        GenerateStatement(forStatement.Body);
        if (!IsLastInstructionOfType<Ret>(_irBuilder.CurrentBlock))
            _irBuilder.Br(incrementBlock);

        _irBuilder.SetBlock(incrementBlock);
        GenerateStatement(forStatement.Increment);
        _irBuilder.Br(conditionBlock);

        _irBuilder.SetBlock(endBlock);
    }

    private void GenerateBreak(Break breakStatement)
    {
        if (_currentLoopEndBlock == null)
        {
            throw new InvalidOperationException("Break statement not inside a loop");
        }

        _irBuilder.Br(_currentLoopEndBlock);
    }

    private void GenerateContinue(Continue continueStatement)
    {
        if (_currentLoopContinueBlock == null)
        {
            throw new InvalidOperationException("Continue statement not inside a loop");
        }

        _irBuilder.Br(_currentLoopContinueBlock);
    }

    private void GenerateExpressionStatement(ExpressionStatement expressionStatement)
    {
        GenerateExpression(expressionStatement.Expression);
    }

    // Expressions
    
    private Value GenerateExpression(Expression expression)
    {
        return expression switch
        {
            Identifier identifierExpression => 
                GenerateIdentifier(identifierExpression),
            NullLiteral nullLiteralExpression => 
                GenerateNullLiteral(nullLiteralExpression),
            IntegerLiteral integerLiteralExpression => 
                GenerateIntegerLiteral(integerLiteralExpression),
            FloatLiteral floatLiteralExpression => 
                GenerateFloatLiteral(floatLiteralExpression),
            StringLiteral stringLiteralExpression => 
                GenerateStringLiteral(stringLiteralExpression),
            BooleanLiteral booleanLiteralExpression => 
                GenerateBooleanLiteral(booleanLiteralExpression),
            UnaryExpression unaryExpression => 
                GenerateUnaryExpression(unaryExpression),
            BinaryExpression binaryExpression => 
                GenerateBinaryExpression(binaryExpression),
            MemberAccessExpression memberAccessExpression => 
                GenerateMemberAccessExpression(memberAccessExpression),
            CallExpression callExpression => 
                GenerateCallExpression(callExpression),
            IndexAccessExpression indexAccessExpression => 
                GenerateIndexAccessExpression(indexAccessExpression),
            CastExpression castExpression => 
                GenerateCastExpression(castExpression),
            DereferenceExpression dereferenceExpression => 
                GenerateDereferenceExpression(dereferenceExpression),
            AddressOfExpression addressOfExpression => 
                GenerateAddressOfExpression(addressOfExpression),
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
    }

    private Value GenerateIdentifier(Identifier identifierExpression)
    {
        var name = identifierExpression.Value;
        var value = _irBuilder.GetValue(name);
        return value;
    }
    
    private Value GenerateNullLiteral(NullLiteral nullLiteralExpression)
    {
        return new Value(new PtrType());
    }

    private Value GenerateIntegerLiteral(IntegerLiteral integerLiteralExpression)
    {
        Debug.Assert(integerLiteralExpression.Type is not null);
        
        var type = MakeType(integerLiteralExpression.Type);
        return new Value(type, constant: integerLiteralExpression.Value);
    }

    private Value GenerateFloatLiteral(FloatLiteral floatLiteralExpression)
    {
        Debug.Assert(floatLiteralExpression.Type is not null);
        
        var type = MakeType(floatLiteralExpression.Type);
        return new Value(type, constant: floatLiteralExpression.Value);
    }

    private Value GenerateStringLiteral(StringLiteral stringLiteralExpression)
    {
        throw new NotImplementedException();
    }

    private Value GenerateBooleanLiteral(BooleanLiteral booleanLiteralExpression)
    {
        Debug.Assert(booleanLiteralExpression.Type is not null);
        
        var type = MakeType(booleanLiteralExpression.Type);
        return new Value(type, constant: booleanLiteralExpression.Value);
    }

    private Value GenerateUnaryExpression(UnaryExpression unaryExpression)
    {
        var operand = GenerateExpression(unaryExpression.Operand);

        return unaryExpression.Operator switch
        {
            TokenType.Bang => throw new NotImplementedException(),
            TokenType.Minus when operand.IRType is IntegerType => _irBuilder.Sub(new Value(operand.IRType, constant: 0), operand),
            TokenType.Minus when operand.IRType is FloatType => _irBuilder.FSub(new Value(operand.IRType, constant: 0), operand),
            TokenType.Tilde when operand.IRType is IntegerType => _irBuilder.Xor(operand, new Value(operand.IRType, constant: -1)),
            _ => throw new NotImplementedException($"Unary operator {unaryExpression.Operator} not implemented")
        };
    }

    private Value GenerateBinaryExpression(BinaryExpression binaryExpression)
    {
        var left = GenerateExpression(binaryExpression.Left);
        left = LoadIfPointer(left);
        var right = GenerateExpression(binaryExpression.Right);
        right = LoadIfPointer(right);
        var leftType = left.IRType;
        var op = binaryExpression.Operator;

        return leftType switch
        {
            IntegerType integerType => GenerateIntegerExpression(op, left, right, integerType),
            FloatType => GenerateFloatExpression(op, left, right),
            BooleanType => GenerateBooleanExpression(op, left, right),
            // PtrType => GeneratePointerExpression(op, left, right),
            PointerType => GeneratePointerExpression(op, left, right),
            _ => throw new NotImplementedException($"Binary operator {op} not implemented for types {leftType} and {right.IRType}")
        };
    }

    private Value LoadIfPointer(Value value)
    {
        if (value.IRType is not PtrType pointerType)
            return value;
        
        return pointerType.Pointee is null ? value : _irBuilder.Load(value);
    }

    private Value GenerateIntegerExpression(TokenType op, Value left, Value right, IntegerType integerType)
    {
        return op switch
        {
            // Arithmetic
            TokenType.Plus => _irBuilder.Add(left, right),
            TokenType.Minus => _irBuilder.Sub(left, right),
            TokenType.Star => _irBuilder.Mul(left, right),
            TokenType.Slash => integerType.IsSigned ? _irBuilder.SDiv(left, right) : _irBuilder.UDiv(left, right),
            // Comparison
            TokenType.EqualEqual => _irBuilder.ICmp("eq", left, right),
            TokenType.BangEqual => _irBuilder.ICmp("ne", left, right),
            TokenType.Greater => _irBuilder.ICmp(integerType.IsSigned ? "sgt" : "ugt", left, right),
            TokenType.GreaterEqual => _irBuilder.ICmp(integerType.IsSigned ? "sge" : "uge", left, right),
            TokenType.Less => _irBuilder.ICmp(integerType.IsSigned ? "slt" : "ult", left, right),
            TokenType.LessEqual => _irBuilder.ICmp(integerType.IsSigned ? "sle" : "ule", left, right),
            // Bitwise
            TokenType.Pipe => _irBuilder.Or(left, right),
            TokenType.Ampersand => _irBuilder.And(left, right),
            TokenType.Caret => _irBuilder.Xor(left, right),
            // Shift
            TokenType.LessLess => _irBuilder.ShiftLeft(left, right),
            TokenType.GreaterGreater => _irBuilder.ShiftRight(left, right),
            _ => throw new NotImplementedException()
        };
    }

    private Value GenerateFloatExpression(TokenType op, Value left, Value right)
    {
        return op switch
        {
            // Arithmetic
            TokenType.Plus => _irBuilder.FAdd(left, right),
            TokenType.Minus => _irBuilder.FSub(left, right),
            TokenType.Star => _irBuilder.FMul(left, right),
            TokenType.Slash => _irBuilder.FDiv(left, right),
            // Comparison
            TokenType.EqualEqual => _irBuilder.FCmp("oeq", left, right),
            TokenType.BangEqual => _irBuilder.FCmp("one", left, right),
            TokenType.Greater => _irBuilder.FCmp("ogt", left, right),
            TokenType.GreaterEqual => _irBuilder.FCmp("oge", left, right),
            TokenType.Less => _irBuilder.FCmp("olt", left, right),
            TokenType.LessEqual => _irBuilder.FCmp("ole", left, right),
            _ => throw new NotImplementedException()
        };
    }

    private Value GenerateBooleanExpression(TokenType op, Value left, Value right)
    {
        return op switch
        {
            TokenType.AmpersandAmpersand => _irBuilder.And(left, right),
            TokenType.PipePipe => _irBuilder.Or(left, right),
            TokenType.EqualEqual => _irBuilder.ICmp("eq", left, right),
            _ => throw new NotImplementedException()
        };
    }
    
    private Value GeneratePointerExpression(TokenType op, Value left, Value right)
    {
        return op switch
        {
            TokenType.Plus => _irBuilder.Add(left, right),
            TokenType.Minus => _irBuilder.Sub(left, right),
            TokenType.EqualEqual => _irBuilder.ICmp("eq", left, right),
            TokenType.BangEqual => _irBuilder.ICmp("ne", left, right),
            _ => throw new NotImplementedException()
        };
    }
    
    private Value GenerateMemberAccessExpression(MemberAccessExpression memberAccessExpression)
    {
        var structInstance = GenerateExpression(memberAccessExpression.Left);
        
        // if (structInstance.IRType is PtrType)
        // {
        //     structInstance = _irBuilder.Load(structInstance);
        // }
        
        if (memberAccessExpression.Right is not Identifier fieldIdentifier)
            throw new InvalidOperationException("Member access expression must have an identifier on the right side.");

        IdentifiedStructType structType;
        if (structInstance.IRType is IdentifiedStructType identifiedStructType)
        {
            structType = identifiedStructType;
        }
        else if (structInstance.IRType is PointerType pointer && pointer.Pointee is IdentifiedStructType identifiedStructTypeThroughPointer)
        {
            structType = identifiedStructTypeThroughPointer;
        }
        else if (structInstance.IRType is PtrType ptr && ptr.Pointee is IdentifiedStructType identifiedStructTypeThroughPtr)
        {
            structType = identifiedStructTypeThroughPtr;
        }
        else if (structInstance.IRType is PtrType ptr2 && ptr2.Pointee is PointerType pointer2 &&
                 pointer2.Pointee is IdentifiedStructType identifiedStructType2)
        {
            structInstance = _irBuilder.Load(structInstance);
            structType = identifiedStructType2;
        }
        else
        {
            throw new InvalidOperationException("Left-hand side of member access does not resolve to a known struct type.");
        }
        
        if (!structType.Fields.ContainsKey(fieldIdentifier.Value))
            throw new InvalidOperationException($"Field '{fieldIdentifier.Value}' not found in struct type.");

        var fieldIndex = structType.Fields.Keys.ToList().IndexOf(fieldIdentifier.Value);

        var gep = _irBuilder.Gep(structInstance, new Value(new IntegerType(32, isSigned: true), constant: 0), new Value(new IntegerType(32, isSigned: true), constant: fieldIndex));

        return gep;
    }

    private Value GenerateCallExpression(CallExpression callExpression)
    {
        var callee = callExpression.Callee as Identifier ?? throw new InvalidOperationException("Callee must be an identifier");
        var function = FindFunction(callee.Value);
        var arguments = callExpression.Arguments.Select(PrepareArgument).ToArray();
        return _irBuilder.Call(function, arguments);
    }

    private IFunction FindFunction(string name)
    {
        var function = _module.Functions.FirstOrDefault(f => f.Name == name);

        if (function is not null)
            return function;
        
        var externFunction = _module.ExternFunctions.FirstOrDefault(f => f.Name == name);
        
        if (externFunction is not null)
            return externFunction;
        
        throw new InvalidOperationException($"Function '{name}' not found");
    }

    private Value PrepareArgument(Argument arg)
    {
        var argValue = GenerateExpression(arg.Expression);

        // Check if the argument is a pointer to an array.
        if (argValue.IRType is PtrType ptrType && ptrType.Pointee is ArrayType)
        {
            // Generate a pointer to the first element of the array
            return _irBuilder.Gep(argValue, new Value(new IntegerType(32, isSigned: true), constant: 0), new Value(new IntegerType(32, isSigned: true), constant: 0));
        }

        if (arg.Expression is AddressOfExpression)
        {
            return argValue;
        }

        // For non-array types or when not passing by reference, potentially load the value
        return ShouldLoad(argValue) ? LoadIfPointer(argValue) : argValue;
    }

    private bool ShouldLoad(Value expression)
    {
        // Implement logic to determine if the expression's value should be loaded,
        // typically true except for array parameters or if passing by reference.
        
        if (expression.IRType is PtrType ptrType && ptrType.Pointee is ArrayType)
            return false;
        
        return true;
    }

    private Value GenerateIndexAccessExpression(IndexAccessExpression indexAccessExpression)
    {
        var arrayValue = GenerateExpression(indexAccessExpression.Left);
        var indexValue = GenerateExpression(indexAccessExpression.Right);

        // Check if arrayValue is a pointer to an array or a scalar type
        if (!(arrayValue.IRType is PtrType pointerType && 
              (pointerType.Pointee is ArrayType || IsScalarType(pointerType.Pointee))))
        {
            throw new InvalidOperationException("Left side of index access expression must be a pointer to an array or a scalar type.");
        }

        // Generate a GEP instruction. 
        // For scalar types, the index access directly computes the offset.
        // For array types, the first index (0) accesses the first element of the array, 
        // and the second index (indexValue) computes the offset within the array.
        Value elementPtr;
        if (IsScalarType(pointerType.Pointee))
        {
            elementPtr = _irBuilder.Gep(arrayValue, indexValue);
        }
        else
        {
            elementPtr = _irBuilder.Gep(arrayValue, new Value(new IntegerType(32, true), constant: 0), indexValue);
        }

        return elementPtr;
    }

    private bool IsScalarType(IrType type)
    {
        // Implement this method to return true if the type is a scalar type (e.g., i32, float)
        // This is a placeholder implementation. Adapt it to your type system.
        return type is IntegerType || type is FloatType; // Add other scalar types as needed
    }

    
    private Value GenerateCastExpression(CastExpression castExpression)
    {
        // Generate the IR for the expression to be casted
        var valueToCast = GenerateExpression(castExpression.Expression);

        // Determine the target type
        var targetType = MakeType(castExpression.TypeRef.Type);

        switch (valueToCast.IRType)
        {
            case PtrType when targetType is PtrType:
            {
                valueToCast.IRType = targetType;
                return valueToCast;
            }
            // Integer to Integer casting (i32 to i64, i64 to i32)
            case IntegerType sourceIntType when targetType is IntegerType targetIntType:
            {
                if (sourceIntType.SizeBits < targetIntType.SizeBits)
                {
                    // Sign extend for increasing integer size
                    return _irBuilder.SExt(valueToCast, targetType);
                }
                if (sourceIntType.SizeBits > targetIntType.SizeBits)
                {
                    // Truncate for decreasing integer size
                    return _irBuilder.Trunc(valueToCast, targetType);
                }

                break;
            }
            // Integer to Floating-point casting (i32 to f32, i64 to f32, i32 to f64, i64 to f64)
            case IntegerType when targetType is FloatType:
                return _irBuilder.SIToFP(valueToCast, targetType);  // Assuming signed integers
            // Floating-point to Integer casting (f32 to i32, f32 to i64, f64 to i32, f64 to i64)
            case FloatType when targetType is IntegerType:
                return _irBuilder.FPToSI(valueToCast, targetType);  // Assuming target is a signed integer
            // Floating-point to Floating-point casting (f32 to f64, f64 to f32)
            case FloatType sourceFloatType when targetType is FloatType targetFloatType:
            {
                if (sourceFloatType.SizeBits < targetFloatType.SizeBits)
                {
                    // Extend for increasing float size
                    return _irBuilder.FPExt(valueToCast, targetType);
                }
                if (sourceFloatType.SizeBits > targetFloatType.SizeBits)
                {
                    // Reduce for decreasing float size
                    return _irBuilder.FPTrunc(valueToCast, targetType);
                }

                break;
            }
        }

        // If no cast is necessary, or it's a cast between the same type, just return the value
        return valueToCast;
    }

    private Value GenerateDereferenceExpression(DereferenceExpression dereferenceExpression)
    {
        var exprPtr = GenerateExpression(dereferenceExpression.Operand);
        
        exprPtr = LoadIfPointer(exprPtr);

        if (exprPtr.IRType is not PointerType)
            throw new InvalidOperationException("Dereference expression must have a pointer type");

        return _irBuilder.Load(exprPtr);
    }

    private Value GenerateAddressOfExpression(AddressOfExpression addressOfExpression)
    {
        var value = GenerateExpression(addressOfExpression.Operand);

        if (value.IRType is not PtrType)
        {
            throw new InvalidOperationException("Address-of expression must have a pointer type");
        }

        return value;
    }
    
    // Helpers
    
    private static bool IsLastInstructionOfType<T>(BasicBlock block) where T : InstructionBase
    {
        if (block.Instructions.Count == 0) return false;
        return block.Instructions[^1] is T;
    }
    
    private string UniqueLabel(string baseLabel)
    {
        _labelCounter.TryAdd(baseLabel, 0);
        _labelCounter[baseLabel]++;
        
        var label = $"{baseLabel}_{_labelCounter[baseLabel]}";
        
        return label;
    }

    private IrType MakeType(Ara.Parsing.Types.Type type)
    {
        switch (type)
        {
            case Parsing.Types.UnknownType:
                throw new ArgumentException("Cannot make an unknown type");
            case Parsing.Types.VoidType:
                return new VoidType();
            case Ara.Parsing.Types.IntType intType:
                return new IntegerType(intType.SizeBits, intType.IsSigned);
            case Ara.Parsing.Types.FloatType floatType:
                return new FloatType(floatType.SizeBits);
            case Parsing.Types.BoolType:
                return new BooleanType();
            case Parsing.Types.StringType:
                throw new NotImplementedException();
            case Ara.Parsing.Types.ArrayType arrayType:
                var elementType = MakeType(arrayType.ElementType);
                return new ArrayType(elementType, arrayType.Size);
            case Ara.Parsing.Types.StructType structType:
                return _irBuilder.GetIdentifiedStruct(structType.Name);
            case Ara.Parsing.Types.PointerType pointerType:
                var pointeeType = MakeType(pointerType.ElementType);
                if (pointeeType is VoidType)
                {
                    pointeeType = new IntegerType(8, true);
                }
                return new PointerType(pointeeType);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}