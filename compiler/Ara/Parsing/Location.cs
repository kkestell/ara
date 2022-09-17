namespace Ara.Parsing;

public struct Location
{
    public Node Node;
    public int Line;
    public int Column;
    public string? Filename;

    public Location(Node node)
    {
        var start = node.Offset.Item1;
        var span = node.Tree.AsSpan(0, start);

        var lines = 1;
        for (var i = 0; i < start; i++)
        {
            if (span[i] == '\n')
                lines++;
        }

        var columns = 1;
        for (var i = span.Length - 1; i >= 0; i--)
        {
            if (span[i] == '\n')
                break;

            columns++;
        }

        Node = node;
        Line = lines;
        Column = columns;
        Filename = node.Tree.Filename;
    }
    
    public string Context
    {
        get
        {
            var offset = Node.Offset.Item1;
            var span = Node.Tree.AsSpan();

            var start = StartOfLine(span, offset);
            var end = EndOfLine(span, offset);

            return Node.Tree.AsSpan(start, end).ToString();
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