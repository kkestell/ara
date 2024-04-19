namespace Ara.Parsing;

public enum TokenType
{
    // End of file
    Eof, 

    // Single-character tokens.
    LeftParen, RightParen, LeftBrace, RightBrace, Comma, Dot, Minus, Plus, Colon, Semicolon, Slash, Star, Percent, Caret, LeftBracket, RightBracket, Ampersand, Pipe, Tilde,

    // One or two character tokens.
    Bang, BangEqual, Equal, EqualEqual, Greater, GreaterEqual, Less, LessEqual, LessLess, GreaterGreater, PipePipe, AmpersandAmpersand,
    
    // Literals.
    Identifier, StringLiteral, IntLiteral, FloatLiteral, BooleanLiteral,

    // Keywords.
    If, Elif, Else, While, Break, Continue, Return, Var, Fn, Class, For, Struct, As, Extern, Null
}