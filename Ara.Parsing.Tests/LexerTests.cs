namespace Ara.Parsing.Tests;

[TestFixture]
public class LexerTests
{
    [Test]
    public void TestIntegerLiteralToken()
    {
        var lexer = new Lexer("123");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.IntLiteral));
        Assert.That(token.Value, Is.InstanceOf<IntegerValue>());
        Assert.That(((IntegerValue)token.Value).Value, Is.EqualTo(123));
    }

    [Test]
    public void TestFloatLiteralToken()
    {
        var lexer = new Lexer("42.5");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.FloatLiteral));
        Assert.That(token.Value, Is.InstanceOf<FloatValue>());
        Assert.That(((FloatValue)token.Value).Value, Is.EqualTo(42.5));
    }
    
    [Test]
    public void TestIdentifierToken()
    {
        var lexer = new Lexer("foobar");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(token.Value, Is.InstanceOf<StringValue>());
        Assert.That(((StringValue)token.Value).Value, Is.EqualTo("foobar"));
    }
    
    [Test]
    public void TestTrue()
    {
        var lexer = new Lexer("true");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.BooleanLiteral));
        Assert.That(token.Value, Is.InstanceOf<BooleanValue>());
    }
    
    [Test]
    public void TestFalse()
    {
        var lexer = new Lexer("false");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.BooleanLiteral));
        Assert.That(token.Value, Is.InstanceOf<BooleanValue>());
    }

    [Test]
    public void TestKeywordToken()
    {
        var lexer = new Lexer("while");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.While));
        Assert.That(token.Value, Is.InstanceOf<NullValue>());
    }
    
    [Test]
    public void TestKeywordReturn()
    {
        var lexer = new Lexer("return");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Return));
        Assert.That(token.Value, Is.InstanceOf<NullValue>());
    }
    
    [Test]
    public void TestKeywordFor()
    {
        var lexer = new Lexer("for");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.For));
        Assert.That(token.Value, Is.InstanceOf<NullValue>());
    }
    
    [Test]
    public void TestOperatorToken()
    {
        var lexer = new Lexer("+");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Plus));
        Assert.That(token.Value, Is.InstanceOf<NullValue>());
    }
    
    [Test]
    public void TestUnexpectedCharacter()
    {
        var lexer = new Lexer("@");
        Assert.Throws<ParsingException>(() => lexer.NextToken());
    }
    
    
    [Test]
    public void TestDoubleCharacterToken()
    {
        var lexer = new Lexer("!=");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.BangEqual));
        Assert.That(token.Value, Is.InstanceOf<NullValue>());
    }
    
    [Test]
    public void TestMultipleTokensWithDoubleCharacter()
    {
        var lexer = new Lexer("if (x != 10) { }");
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.If));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.LeftParen));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.BangEqual));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.IntLiteral));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.RightParen));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.LeftBrace));
        Assert.That(lexer.NextToken().Type, Is.EqualTo(TokenType.RightBrace));
    }
    
    [Test]
    public void TestStringToken()
    {
        var lexer = new Lexer("\"Hello, world!\"");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.StringLiteral));
        Assert.That(token.Value, Is.InstanceOf<StringValue>());
        Assert.That(((StringValue)token.Value).Value, Is.EqualTo("Hello, world!"));
    }
    
    [Test]
    public void TestNumberTokenLocation()
    {
        var lexer = new Lexer("\n\n  123");
        
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.IntLiteral));
        Assert.That(token.Value, Is.InstanceOf<IntegerValue>());
        Assert.That(((IntegerValue)token.Value).Value, Is.EqualTo(123));
        Assert.That(token.Location.Row, Is.EqualTo(3));
        Assert.That(token.Location.Column, Is.EqualTo(3));
    }

    [Test]
    public void TestMultipleTokens()
    {
        const string input = "var x: int = 123.5;";
        var lexer = new Lexer(input);

        var token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.Var, new Location(1, 1, input.AsMemory()))));

        token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.Identifier, new Location(1, 5, input.AsMemory()), "x")));

        token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.Colon, new Location(1, 6, input.AsMemory()))));

        token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.Identifier, new Location(1, 8, input.AsMemory()), "int")));

        token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.Equal, new Location(1, 12, input.AsMemory()))));

        token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.FloatLiteral, new Location(1, 14, input.AsMemory()), 123.5f)));

        token = lexer.NextToken();
        Assert.That(token, Is.EqualTo(new Token(TokenType.Semicolon, new Location(1, 19, input.AsMemory()))));
    }

    [Test]
    public void TestWhitespaceToken()
    {
        var lexer = new Lexer(" 123 ");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.IntLiteral));
        Assert.That(token.Value, Is.InstanceOf<IntegerValue>());
        Assert.That(((IntegerValue)token.Value).Value, Is.EqualTo(123));
    }

    [Test]
    public void TestEscapedStringToken()
    {
        var lexer = new Lexer("\"Hello\\nWorld\"");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.StringLiteral));
        Assert.That(token.Value, Is.InstanceOf<StringValue>());
        Assert.That(((StringValue)token.Value).Value, Is.EqualTo("Hello\\nWorld"));
    }

    [Test]
    public void TestDoubleQuoteInStringToken()
    {
        var lexer = new Lexer("\"Hello \\\"World\\\"\"");
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.StringLiteral));
        Assert.That(token.Value, Is.InstanceOf<StringValue>());
        Assert.That(((StringValue)token.Value).Value, Is.EqualTo("Hello \\\"World\\\""));
    }

    [Test]
    public void TestStringType()
    {
        var lexer = new Lexer("var x: string = \"hello world\";");
        
        var token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Var));
        
        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Identifier));
        
        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Colon));
        
        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Identifier));
        
        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Equal));
        
        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.StringLiteral));
        
        token = lexer.NextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Semicolon));
    }

    [Test]
    public void TestUnterminatedString()
    {
        const string input = "\"Hello, world";
        var lexer = new Lexer(input);

        var ex = Assert.Throws<ParsingException>(() => { while (lexer.NextToken().Type != TokenType.Eof); });

        Assert.That(ex.Message, Is.EqualTo("\"Hello, world\n^\n(1,1) Unterminated string."));
        Assert.That(ex.Location.Row, Is.EqualTo(1));
        Assert.That(ex.Location.Column, Is.EqualTo(1));
        Assert.That(ex.Location.LineContent, Is.EqualTo(input.AsMemory()));
    }

    [Test]
    public void TestUnexpectedCharacterSecondRow()
    {
        var lexer = new Lexer("foo\n#");

        var ex = Assert.Throws<ParsingException>(() =>
        {
            while (true)
            {
                var nextToken = lexer.NextToken();

                if (nextToken.Type == TokenType.Eof)
                {
                    break;
                }
            }
        });

        Assert.That(ex.Message, Is.EqualTo("#\n^\n(2,1) Unexpected character: #"));
        Assert.That(ex.Location.Row, Is.EqualTo(2));
        Assert.That(ex.Location.Column, Is.EqualTo(1));
        Assert.That(ex.Location.LineContent.ToString(), Is.EqualTo("#"));
    }

    [Test]
    public void TestUnterminatedStringWithNewLineMiddle()
    {
        var lexer = new Lexer("foo\n\"Hello, world\nbar");

        var ex = Assert.Throws<ParsingException>(() => { while (lexer.NextToken().Type != TokenType.Eof); });

        Assert.That(ex.Message, Is.EqualTo("\"Hello, world\n^\n(2,1) Unterminated string."));
        Assert.That(ex.Location.Row, Is.EqualTo(2));
        Assert.That(ex.Location.Column, Is.EqualTo(1));
        Assert.That(ex.Location.LineContent.ToString(), Is.EqualTo("\"Hello, world"));
    }

    [Test]
    public void TestUnexpectedCharacterInMiddleOfLine()
    {
        const string input = "foo # bar";
        var lexer = new Lexer(input);

        var ex = Assert.Throws<ParsingException>(() => { while (lexer.NextToken().Type != TokenType.Eof); });

        Assert.That(ex.Message, Is.EqualTo("foo # bar\n----^\n(1,5) Unexpected character: #"));
        Assert.That(ex.Location.Row, Is.EqualTo(1));
        Assert.That(ex.Location.Column, Is.EqualTo(5));
        Assert.That(ex.Location.LineContent, Is.EqualTo(input.AsMemory()));
    }
}
