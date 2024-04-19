using Ara.Parsing.Nodes;

namespace Ara.Parsing;

public class Parser
{
    private readonly string? _filename;
    private readonly Lexer _lexer;
    private Token _currentToken;
    private Token _nextToken;

    public Parser(Lexer lexer, string? filename = null)
    {
        _lexer = lexer;
        _filename = filename;

        _currentToken = _lexer.NextToken();
        _nextToken = _lexer.NextToken();
    }

    private Location BuildLocation()
    {
        return _currentToken.Location with { FileName = _filename };
    }

    public CompilationUnit ParseUnit()
    {
        var location = BuildLocation();

        var definitions = new List<Definition>();

        while (_currentToken.Type != TokenType.Eof)
            if (_currentToken.Type == TokenType.Extern)
                definitions.Add(ExternFunctionDefinition());
            else if (_currentToken.Type == TokenType.Fn)
                definitions.Add(ParseFunctionDefinition());
            else if (_currentToken.Type == TokenType.Struct)
                definitions.Add(ParseStructDefinition());
            else
                throw new ParsingException("Unable to parse definition", _currentToken.Location);

        return new CompilationUnit(definitions, location);
    }

    // Misc.

    private Identifier ParseIdentifier()
    {
        var location = BuildLocation();

        var identifierToken = ConsumeToken(TokenType.Identifier);

        if (identifierToken.Value is not StringValue stringLiteral)
            throw new ParsingException("Token value should be a string", _currentToken.Location);

        return new Identifier(stringLiteral.Value, location);
    }

    private TypeRef ParseTypeRef()
    {
        var location = BuildLocation();

        if (_currentToken.Type == TokenType.LeftBracket)
        {
            ConsumeToken(TokenType.LeftBracket);
            // FIXME: Size should be an expression
            var sizeToken = ConsumeToken(TokenType.IntLiteral);
            if (sizeToken.Value is not IntegerValue intValue)
                throw new ParsingException("Token value should be an integer", _currentToken.Location);
            ConsumeToken(TokenType.RightBracket);
            var baseType = ParseTypeRef();
            return new ArrayTypeRef(baseType, intValue.Value, location);
        }

        if (_currentToken.Type == TokenType.Caret)
        {
            ConsumeToken(TokenType.Caret);
            var baseType = ParseTypeRef();
            return new PointerTypeRef(baseType, location);
        }

        var identifier = ParseIdentifier();
        return new BasicTypeRef(identifier.Value, location);
    }

    // Literals

    private NullLiteral ParseNullLiteral()
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.Null);
        return new NullLiteral(location);
    }

    private IntegerLiteral ParseIntegerLiteral()
    {
        var location = BuildLocation();

        var numberToken = ConsumeToken(TokenType.IntLiteral);

        if (numberToken.Value is not IntegerValue intValue)
            throw new ParsingException("Token value should be an integer", _currentToken.Location);

        return new IntegerLiteral(intValue.Value, location);
    }

    private FloatLiteral ParseFloatLiteral()
    {
        var location = BuildLocation();

        var numberToken = ConsumeToken(TokenType.FloatLiteral);

        if (numberToken.Value is not FloatValue floatValue)
            throw new ParsingException("Token value should be a float", _currentToken.Location);

        return new FloatLiteral(floatValue.Value, location);
    }

    private StringLiteral ParseStringLiteral()
    {
        var location = BuildLocation();

        var stringToken = ConsumeToken(TokenType.StringLiteral);

        if (stringToken.Value is not StringValue stringValue)
            throw new ParsingException("Token value should be a string", _currentToken.Location);

        return new StringLiteral(stringValue.Value, location);
    }

    private BooleanLiteral ParseBooleanLiteral()
    {
        var location = BuildLocation();

        var boolean = ConsumeToken(TokenType.BooleanLiteral);

        if (boolean.Value is not BooleanValue booleanValue)
            throw new ParsingException("Token value should be a boolean", _currentToken.Location);

        return new BooleanLiteral(booleanValue.Value, location);
    }

    // Declarations

    // Functions

    private ExternFunctionDefinition ExternFunctionDefinition()
    {
        var location = BuildLocation();

        ConsumeToken(TokenType.Extern);
        ConsumeToken(TokenType.Fn);
        var identifier = ParseIdentifier();
        ConsumeToken(TokenType.LeftParen);
        var parameters = ParseParameters();
        ConsumeToken(TokenType.RightParen);
        ConsumeToken(TokenType.Colon);
        var type = ParseTypeRef();

        return new ExternFunctionDefinition(identifier.Value, parameters, type, location);
    }

    private FunctionDefinition ParseFunctionDefinition()
    {
        var location = BuildLocation();

        ConsumeToken(TokenType.Fn);
        var identifier = ParseIdentifier();
        ConsumeToken(TokenType.LeftParen);
        var parameters = ParseParameters();
        ConsumeToken(TokenType.RightParen);
        ConsumeToken(TokenType.Colon);
        var type = ParseTypeRef();
        var body = ParseBlock();

        return new FunctionDefinition(identifier.Value, parameters, type, body, location);
    }

    private List<Parameter> ParseParameters()
    {
        var parameters = new List<Parameter>();
        while (_currentToken.Type != TokenType.RightParen)
        {
            parameters.Add(ParseParameter());
            if (_currentToken.Type == TokenType.Comma) ConsumeToken(TokenType.Comma);
        }

        return parameters;
    }

    private Parameter ParseParameter()
    {
        var location = BuildLocation();

        var identifier = ParseIdentifier();
        ConsumeToken(TokenType.Colon);
        var type = ParseTypeRef();
        return new Parameter(identifier.Value, type, location);
    }

    // Structs

    private StructDefinition ParseStructDefinition()
    {
        var location = BuildLocation();

        ConsumeToken(TokenType.Struct);
        var identifier = ParseIdentifier();
        ConsumeToken(TokenType.LeftBrace);
        var fields = ParseStructFields();
        ConsumeToken(TokenType.RightBrace);
        return new StructDefinition(identifier.Value, fields, location);
    }

    private List<StructField> ParseStructFields()
    {
        var fields = new List<StructField>();
        while (_currentToken.Type != TokenType.RightBrace) fields.Add(ParseStructField());
        return fields;
    }

    private StructField ParseStructField()
    {
        var location = BuildLocation();

        var identifier = ParseIdentifier();
        ConsumeToken(TokenType.Colon);
        var type = ParseTypeRef();
        return new StructField(identifier.Value, type, location);
    }

    // Statements

    private Statement ParseStatement()
    {
        if (_currentToken.Type == TokenType.LeftBrace)
            return ParseBlock();

        if (_currentToken.Type == TokenType.Identifier)
        {
            var expr = ParseExpression();
            if (_currentToken.Type == TokenType.Equal)
                return ParseAssignment(expr);
            
            var location = BuildLocation();
            return new ExpressionStatement(expr, location);
        }
        
        Statement statement = _currentToken.Type switch
        {
            TokenType.Var => ParseVariableDeclaration(),
            TokenType.Return => ParseReturn(),
            TokenType.If => ParseIf(),
            TokenType.While => ParseWhile(),
            TokenType.For => ParseFor(),
            TokenType.Break => ParseBreak(),
            TokenType.Continue => ParseContinue(),
            _ => throw new ParsingException($"Unexpected {_currentToken.Type}", _currentToken.Location)
        };
        
        if (_currentToken.Type == TokenType.Semicolon)
            ConsumeToken(TokenType.Semicolon);
        
        return statement;
    }

    private Block ParseBlock()
    {
        var location = BuildLocation();

        ConsumeToken(TokenType.LeftBrace);
        var statements = new List<Statement>();
        while (_currentToken.Type != TokenType.RightBrace) statements.Add(ParseStatement());
        ConsumeToken(TokenType.RightBrace);

        return new Block(statements, location);
    }

    private VariableDeclaration ParseVariableDeclaration()
    {
        var location = BuildLocation();

        ConsumeToken(TokenType.Var);
        var identifierNode = ParseIdentifier();
        ConsumeToken(TokenType.Colon);
        var type = ParseTypeRef();

        Expression? expression = null;
        if (_currentToken.Type == TokenType.Equal)
        {
            ConsumeToken(TokenType.Equal);
            expression = ParseExpression();
        }
        
        return new VariableDeclaration(identifierNode.Value, type, expression, location);
    }

    private Return ParseReturn()
    {
        var location = BuildLocation();

        ConsumeToken(TokenType.Return);

        if (_currentToken.Type == TokenType.Semicolon)
        {
            ConsumeToken(TokenType.Semicolon);
            return new Return(null, location);
        }

        var expressionNode = ParseExpression();
        
        return new Return(expressionNode, location);
    }

    private If ParseIf(bool isElif = false)
    {
        var location = BuildLocation();

        if (isElif)
            ConsumeToken(TokenType.Elif);
        else
            ConsumeToken(TokenType.If);

        ConsumeToken(TokenType.LeftParen);
        var condition = ParseExpression();
        ConsumeToken(TokenType.RightParen);
        var body = ParseStatement();

        Statement? elseBlock = null;

        if (_currentToken.Type == TokenType.Elif)
        {
            elseBlock = ParseIf(true);
        }
        else if (_currentToken.Type == TokenType.Else)
        {
            ConsumeToken(TokenType.Else);
            elseBlock = ParseStatement();
        }

        return new If(condition, body, elseBlock, location);
    }

    private While ParseWhile()
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.While);
        ConsumeToken(TokenType.LeftParen);
        var condition = ParseExpression();
        ConsumeToken(TokenType.RightParen);
        var body = ParseStatement();
        return new While(condition, body, location);
    }

    private For ParseFor()
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.For);
        ConsumeToken(TokenType.LeftParen);
        var initializer = ParseStatement();
        var condition = ParseExpression();
        ConsumeToken(TokenType.Semicolon);
        var increment = ParseStatement();
        ConsumeToken(TokenType.RightParen);
        var body = ParseBlock();
        return new For(initializer, condition, increment, body, location);
    }

    private Break ParseBreak()
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.Break);
        return new Break(location);
    }

    private Continue ParseContinue()
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.Continue);
        return new Continue(location);
    }

    private Assignment ParseAssignment(Expression left)
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.Equal);
        var value = ParseExpression();
        return new Assignment(left, value, location);
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        var location = BuildLocation();
        var expression = ParseExpression();
        ConsumeToken(TokenType.Semicolon);
        return new ExpressionStatement(expression, location);
    }

    // Expressions

    private Expression ParseExpression()
    {
        return ParseExpression(8);
    }

    private Expression ParseExpression(int precedence)
    {
        Expression node;

        if (precedence == 1)
            node = ParseUnary();
        else
            node = ParseExpression(precedence - 1);

        while (IsBinaryOperatorOfPrecedence(_currentToken.Type, precedence))
        {
            var location = BuildLocation();
            var op = _currentToken.Type;
            ConsumeToken();
            var right = ParseExpression(precedence - 1);
            node = new BinaryExpression(node, op, right, location);
        }

        return node;
    }

    private Expression ParseUnary()
    {
        while (IsUnaryOperator(_currentToken.Type) || _currentToken.Type == TokenType.LeftParen)
        {
            var location = BuildLocation();

            if (_currentToken.Type == TokenType.Ampersand)
            {
                ConsumeToken(TokenType.Ampersand);
                var operand = ParseUnary();
                return new AddressOfExpression(operand, location);
            }

            if (_currentToken.Type == TokenType.Caret)
            {
                ConsumeToken(TokenType.Caret);
                var operand = ParseUnary();
                return new DereferenceExpression(operand, location);
            }

            if (IsUnaryOperator(_currentToken.Type))
            {
                var op = _currentToken.Type;
                ConsumeToken();
                var operand = ParseUnary();
                return new UnaryExpression(op, operand, location);
            }

            if (_currentToken.Type == TokenType.LeftParen)
            {
                ConsumeToken(TokenType.LeftParen);
                var expression = ParseExpression();
                ConsumeToken(TokenType.RightParen);
                return expression;
            }
        }

        return ParseAtom();
    }
    
    private Expression ParseAtom()
    {
        Expression node;

        // Parse the base expression
        if (_currentToken.Type == TokenType.LeftParen)
        {
            node = ParseParenthesizedExpression();
        }
        else if (_currentToken.Type == TokenType.Null)
        {
            node = ParseNullLiteral();
        }
        else if (_currentToken.Type == TokenType.IntLiteral)
        {
            node = ParseIntegerLiteral();
        }
        else if (_currentToken.Type == TokenType.FloatLiteral)
        {
            node = ParseFloatLiteral();
        }
        else if (_currentToken.Type == TokenType.StringLiteral)
        {
            node = ParseStringLiteral();
        }
        else if (_currentToken.Type == TokenType.BooleanLiteral)
        {
            node = ParseBooleanLiteral();
        }
        else if (_currentToken.Type == TokenType.Identifier)
        {
            node = ParseIdentifier();
        }
        else if (_currentToken.Type == TokenType.LeftBracket)
        {
            node = ParseArrayInitializationExpression();
        }
        else
        {
            throw new ParsingException($"Unexpected {_currentToken.Type}", _currentToken.Location);
        }

        // Loop to handle postfix expressions like index access, member access, casting, and function calls
        while (true)
        {
            if (_currentToken.Type == TokenType.LeftBracket)
            {
                // Parse index expression
                node = ParseIndexExpression(node);
            }
            else if (_currentToken.Type == TokenType.As)
            {
                // Parse cast expression
                node = ParseCastExpression(node);
            }
            else if (_currentToken.Type == TokenType.Dot)
            {
                // Parse member access expression
                node = ParseMemberAccessExpression(node);
            }
            else if (_currentToken.Type == TokenType.LeftParen)
            {
                // Parse function call expression
                node = ParseCallExpression(node);
            }
            else if (_currentToken.Type == TokenType.Caret)
            {
                // Parse dereference expression
                var location = BuildLocation();
                ConsumeToken(TokenType.Caret); // Consume the '^' token
                node = new DereferenceExpression(node, location);
            }
            else
            {
                break; // No more postfix expressions
            }
        }
        
        return node;
    }

    private Expression ParseParenthesizedExpression()
    {
        ConsumeToken(TokenType.LeftParen);
        var node = ParseExpression();
        ConsumeToken(TokenType.RightParen);
        return node;
    }

    private MemberAccessExpression ParseMemberAccessExpression(Expression expression)
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.Dot);
        var memberName = ParseIdentifier();
        return new MemberAccessExpression(expression, memberName, location);
    }

    private CallExpression ParseCallExpression(Expression callee)
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.LeftParen);
        var arguments = ParseArguments();
        ConsumeToken(TokenType.RightParen);
        return new CallExpression(callee, arguments, location);
    }

    private IndexAccessExpression ParseIndexExpression(Expression expression)
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.LeftBracket);
        var index = ParseExpression();
        ConsumeToken(TokenType.RightBracket);
        return new IndexAccessExpression(expression, index, location);
    }

    private CastExpression ParseCastExpression(Expression expression)
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.As);
        var type = ParseTypeRef();
        return new CastExpression(type, expression, location);
    }

    private List<Argument> ParseArguments()
    {
        var arguments = new List<Argument>();
        while (_currentToken.Type != TokenType.RightParen)
        {
            arguments.Add(ParseArgument());
            if (_currentToken.Type == TokenType.Comma) ConsumeToken(TokenType.Comma);
        }

        return arguments;
    }

    private Argument ParseArgument()
    {
        var location = BuildLocation();
        var expression = ParseExpression();
        return new Argument(expression, location);
    }

    private Expression ParseArrayInitializationExpression()
    {
        var location = BuildLocation();
        ConsumeToken(TokenType.LeftBracket);
        var elements = new List<Expression>();
        if (_currentToken.Type != TokenType.RightBracket)
            while (true)
            {
                elements.Add(ParseExpression());
                if (_currentToken.Type == TokenType.Comma)
                    ConsumeToken(TokenType.Comma);
                else
                    break;
            }

        ConsumeToken(TokenType.RightBracket);
        return new ArrayInitializationExpression(elements, location);
    }

    // Helpers

    private Token ConsumeToken()
    {
        var token = _currentToken;
        _currentToken = _nextToken;
        _nextToken = _lexer.NextToken();
        return token;
    }

    private Token ConsumeToken(TokenType expectedType)
    {
        if (_currentToken.Type == expectedType) return ConsumeToken();

        throw new ParsingException($"Unexpected token: Expected {expectedType}, got {_currentToken.Type}",
            _currentToken.Location);
    }

    private static bool IsUnaryOperator(TokenType type)
    {
        var unaryOperators = new List<TokenType>
        {
            TokenType.Minus, TokenType.Bang, TokenType.Tilde, TokenType.Caret, TokenType.Ampersand
        };
        return unaryOperators.Contains(type);
    }

    private static bool IsBinaryOperatorOfPrecedence(TokenType type, int precedence)
    {
        var precedenceMap = new Dictionary<int, List<TokenType>>
        {
            { 2, new List<TokenType> { TokenType.Star, TokenType.Slash, TokenType.Percent } },
            { 3, new List<TokenType> { TokenType.Plus, TokenType.Minus } },
            { 4, new List<TokenType> { TokenType.LessLess, TokenType.GreaterGreater } },
            { 5, new List<TokenType> { TokenType.Ampersand, TokenType.Caret, TokenType.Pipe } },
            {
                6,
                new List<TokenType>
                {
                    TokenType.EqualEqual, TokenType.BangEqual, TokenType.Less, TokenType.LessEqual, TokenType.Greater,
                    TokenType.GreaterEqual
                }
            },
            { 7, new List<TokenType> { TokenType.AmpersandAmpersand } },
            { 8, new List<TokenType> { TokenType.PipePipe } }
        };

        return precedenceMap.TryGetValue(precedence, out var operators) && operators.Contains(type);
    }
}