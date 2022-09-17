using System.Runtime.InteropServices;

namespace Ara.Parsing;

public sealed class Parser : IDisposable
{
    readonly Handle<Parser> handle;

    public Parser()
    {
        handle = create_parser();
    }

    public void Dispose()
    {
        delete_parser(handle);
    }

    public Tree Parse(string source, string? filename = null)
    {
        var tree = parse(handle, source);
        return new Tree(tree, source, filename);
    }

    [DllImport("parser.so")]
    static extern Handle<Parser> create_parser();

    [DllImport("parser.so")]
    static extern void delete_parser(Handle<Parser> parser);

    [DllImport("parser.so", CharSet = CharSet.Ansi)]
    static extern Handle<Tree> parse(Handle<Parser> parser, string source);
}