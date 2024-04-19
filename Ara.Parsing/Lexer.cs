using System.Text;

namespace Ara.Parsing;

public class Lexer
{
    private readonly InputBuffer _inputBuffer;
    private readonly StringBuilder _stringBuilder = new(256);

    public Lexer(string input)
    {
        _inputBuffer = new InputBuffer(input);
    }

    public Token NextToken()
    {
        while (true)
        {
            while (!_inputBuffer.IsEmpty() && char.IsWhiteSpace(_inputBuffer.Peek()))
            {
                _inputBuffer.Pop();
            }

            if (_inputBuffer.IsEmpty())
            {
                return new Token(TokenType.Eof, _inputBuffer.Location);
            }

            var ch = _inputBuffer.Peek();
            
            if (ch == '/' && _inputBuffer.Peek(1) == '/')
            {
                while (!_inputBuffer.IsEmpty() && _inputBuffer.Peek() != '\n')
                {
                    _inputBuffer.Pop();
                }

                continue;
            }

            if (char.IsNumber(ch))
            {
                return ReadNumber();
            }

            if (char.IsLetter(ch))
            {
                return ReadIdentifierOrKeyword();
            }

            if (ch is '\"' or '\'')
            {
                return ReadString(ch);
            }

            if (ch == '/' && _inputBuffer.Peek(1) == '/')
            {
                while (!_inputBuffer.IsEmpty() && _inputBuffer.Peek() != '\n')
                {
                    _inputBuffer.Pop();
                }

                continue;
            }

            var location = _inputBuffer.Location;

            if (ch == '[')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.LeftBracket, location);
            }
            
            if (ch == ']')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.RightBracket, location);
            }
            
            if (ch == '^')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Caret, location);
            }
            
            if (ch == '(')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.LeftParen, location);
            }

            if (ch == ')')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.RightParen, location);
            }

            if (ch == '{')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.LeftBrace, location);
            }

            if (ch == '}')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.RightBrace, location);
            }

            if (ch == ',')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Comma, location);
            }

            if (ch == '.')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Dot, location);
            }

            if (ch == '-')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Minus, location);
            }

            if (ch == '+')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Plus, location);
            }

            if (ch == ':')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Colon, location);
            }

            if (ch == ';')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Semicolon, location);
            }

            if (ch == '/')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Slash, location);
            }

            if (ch == '*')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Star, location);
            }

            if (ch == '%')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Percent, location);
            }
            
            if (ch == '&')
            {
                if (_inputBuffer.Peek(1) == '&')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.AmpersandAmpersand, location);
                }

                _inputBuffer.Pop();
                return new Token(TokenType.Ampersand, location);
            }

            if (ch == '|')
            {
                if (_inputBuffer.Peek(1) == '|')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.PipePipe, location);
                }

                _inputBuffer.Pop();
                return new Token(TokenType.Pipe, location);
            }
            
            if (ch == '~')
            {
                _inputBuffer.Pop();
                return new Token(TokenType.Tilde, location);
            }

            if (ch == '!')
            {
                if (_inputBuffer.Peek(1) == '=')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.BangEqual, location);
                }

                _inputBuffer.Pop();
                return new Token(TokenType.Bang, location);
            }

            if (ch == '=')
            {
                if (_inputBuffer.Peek(1) == '=')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.EqualEqual, location);
                }

                _inputBuffer.Pop();
                return new Token(TokenType.Equal, location);
            }

            if (ch == '<')
            {
                if (_inputBuffer.Peek(1) == '=')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.LessEqual, location);
                }
                
                if (_inputBuffer.Peek(1) == '<')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.LessLess, location);
                }

                _inputBuffer.Pop();
                return new Token(TokenType.Less, location);
            }

            if (ch == '>')
            {
                if (_inputBuffer.Peek(1) == '=')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.GreaterEqual, location);
                }
               
                if (_inputBuffer.Peek(1) == '>')
                {
                    _inputBuffer.Pop();
                    _inputBuffer.Pop();
                    return new Token(TokenType.GreaterGreater, location);
                }

                _inputBuffer.Pop();
                return new Token(TokenType.Greater, location);
            }

            throw new ParsingException($"Unexpected character: {ch}", location);
        }
    }

    private Token ReadNumber()
    {
        var loc = _inputBuffer.Location;

        _stringBuilder.Clear();

        if (_inputBuffer.Peek() == '0' && (_inputBuffer.Peek(1) == 'x' || _inputBuffer.Peek(1) == 'X'))
        {
            _stringBuilder.Append(_inputBuffer.Peek());
            _inputBuffer.Pop();
            _stringBuilder.Append(_inputBuffer.Peek());
            _inputBuffer.Pop();

            while (!_inputBuffer.IsEmpty() && (char.IsNumber(_inputBuffer.Peek()) || IsHexChar(_inputBuffer.Peek())))
            {
                _stringBuilder.Append(_inputBuffer.Peek());
                _inputBuffer.Pop();
            }

            return new Token(TokenType.IntLiteral, loc, Convert.ToInt32(_stringBuilder.ToString(), 16));
        }

        while (!_inputBuffer.IsEmpty() && char.IsNumber(_inputBuffer.Peek()))
        {
            _stringBuilder.Append(_inputBuffer.Peek());
            _inputBuffer.Pop();
        }

        if (_inputBuffer.Peek() != '.')
        {
            return new Token(TokenType.IntLiteral, loc, int.Parse(_stringBuilder.ToString()));
        }

        if (char.IsNumber(_inputBuffer.Peek(1)))
        {
            _stringBuilder.Append(_inputBuffer.Peek());
            _inputBuffer.Pop();

            while (!_inputBuffer.IsEmpty() && char.IsNumber(_inputBuffer.Peek()))
            {
                _stringBuilder.Append(_inputBuffer.Peek());
                _inputBuffer.Pop();
            }
        }

        return new Token(TokenType.FloatLiteral, loc, float.Parse(_stringBuilder.ToString()));
    }

    private bool IsHexChar(char c)
    {
        return (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
    }

    private Token ReadIdentifierOrKeyword()
    {
        var loc = _inputBuffer.Location;
        
        _stringBuilder.Clear();
        
        while (!_inputBuffer.IsEmpty() && IsIdentifierChar(_inputBuffer.Peek()))
        {
            _stringBuilder.Append(_inputBuffer.Peek());
            _inputBuffer.Pop();
        }
        var identifier = _stringBuilder.ToString();

        if (identifier == "if")
        {
            return new Token(TokenType.If, loc);
        }

        if (identifier == "elif")
        {
            return new Token(TokenType.Elif, loc);
        }

        if (identifier == "else")
        {   
            return new Token(TokenType.Else, loc);
        }

        if (identifier == "while")
        {
            return new Token(TokenType.While, loc);
        }

        if (identifier == "break")
        {
            return new Token(TokenType.Break, loc);
        }

        if (identifier == "continue")
        {
            return new Token(TokenType.Continue, loc);
        }

        if (identifier == "true")
        {
            return new Token(TokenType.BooleanLiteral, loc, true);
        }

        if (identifier == "false")
        {
            return new Token(TokenType.BooleanLiteral, loc, false);
        }

        if (identifier == "return")
        {
            return new Token(TokenType.Return, loc);
        }

        if (identifier == "var")
        {
            return new Token(TokenType.Var, loc);
        }

        if (identifier == "fn")
        {
            return new Token(TokenType.Fn, loc);
        }
        
        if (identifier == "class")
        {
            return new Token(TokenType.Class, loc);
        }

        if (identifier == "for")
        {
            return new Token(TokenType.For, loc);
        }

        if (identifier == "struct")
        {
            return new Token(TokenType.Struct, loc);
        }

        if (identifier == "as")
        {
            return new Token(TokenType.As, loc);
        }
        
        if (identifier == "extern")
        {
            return new Token(TokenType.Extern, loc);
        }
        
        if (identifier == "null")
        {
            return new Token(TokenType.Null, loc);
        }
        
        return new Token(TokenType.Identifier, loc, identifier);
    }

    private Token ReadString(char quote)
    {
        var location = _inputBuffer.Location;

        _inputBuffer.Pop();

        _stringBuilder.Clear();

        while (true)
        {
            var ch = _inputBuffer.Peek();

            if (ch is '\n' or '\0')
            {
                throw new ParsingException("Unterminated string.", location);
            }

            if (ch == quote)
            {
                _inputBuffer.Pop();
                break;
            }
            
            if (ch == '\\')
            {
                _inputBuffer.Pop();
                _stringBuilder.Append('\\');
                _stringBuilder.Append(_inputBuffer.Peek());
                _inputBuffer.Pop();
            }
            else
            {
                _stringBuilder.Append(ch);
                _inputBuffer.Pop();
            }
        }

        return new Token(TokenType.StringLiteral, location, _stringBuilder.ToString());
    }

    private static bool IsIdentifierChar(char ch)
    {
        return char.IsLetterOrDigit(ch) || ch == '_';
    }
}