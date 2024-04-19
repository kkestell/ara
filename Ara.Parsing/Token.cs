namespace Ara.Parsing;

public interface IValue { }

public sealed record NullValue() : IValue;
public sealed record IntegerValue(int Value) : IValue;
public sealed record FloatValue(float Value) : IValue;
public sealed record StringValue(string Value) : IValue;
public sealed record BooleanValue(bool Value) : IValue;

public record Token(TokenType Type, Location Location, IValue Value)
{
    public Token(TokenType type, Location location) : this(type, location, new NullValue()) { }
    public Token(TokenType type, Location location, string value) : this(type, location, new StringValue(value)) { }
    public Token(TokenType type, Location location, int value) : this(type, location, new IntegerValue(value)) { }
    public Token(TokenType type, Location location, float value) : this(type, location, new FloatValue(value)) { }
    public Token(TokenType type, Location location, bool value) : this(type, location, new BooleanValue(value)) { }
}