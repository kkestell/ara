namespace Ara.Parsing;

public readonly struct Location
{
    public Location(ParseNode parseNode)
    {
        var pt = parseNode.StartPoint;
        
        ParseNode = parseNode;
        Line = pt.Row;
        Column = pt.Column;
        Filename = parseNode.Tree.Filename;
    }
    
    public ParseNode ParseNode { get; }
    
    public int Line { get; }
    
    public int Column { get; }
    
    public string? Filename { get; }
    
    public string Context
    {
        get
        {
            var offset = ParseNode.Offset.Item1;
            var span = ParseNode.Tree.AsSpan();

            var start = StartOfLine(span, offset);
            var end = EndOfLine(span, offset);

            return ParseNode.Tree.AsSpan(start, end).ToString();
        }
    }

    public override string ToString()
    {
        return $"{Filename ?? "<source>"}:{Line}:{Column}";
    }
    
    static int StartOfLine(ReadOnlySpan<char> span, int offset)
    {
        var start = 0;
        var lines = 0;
        
        for (var i = offset; i >= 0; i--)
        {
            if (span[i] != '\n') continue;
            
            lines++;
            
            if (lines != 2) continue;
            
            start = i + 1;
            break;
        }

        return start;
    }

    static int EndOfLine(ReadOnlySpan<char> span, int offset)
    {
        var end = 0;
        
        for (var i = offset; i < span.Length; i++)
        {
            if (span[i] != '\n') continue;
            
            end = i;
            break;
        }

        return end;
    }
}