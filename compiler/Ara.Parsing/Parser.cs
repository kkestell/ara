using System.Runtime.InteropServices;

namespace Ara.Parsing;

public sealed class Parser : IDisposable
{
    readonly Handle<Parser> handle;

    public Parser()
    {
        handle = CreateParser();
    }

    public void Dispose()
    {
        DeleteParser(handle);
    }

    public Tree Parse(string source, string? filename = null)
    {
        var tree = Parse(handle, source);
        return new Tree(tree, source, filename);
    }

    [DllImport(Platform.SharedLibrary, EntryPoint = "create_parser")]
    static extern Handle<Parser> CreateParser();

    [DllImport(Platform.SharedLibrary, EntryPoint = "delete_parser")]
    static extern void DeleteParser(Handle<Parser> parser);

    [DllImport(Platform.SharedLibrary, EntryPoint = "parse", CharSet = CharSet.Ansi)]
    static extern Handle<Tree> Parse(Handle<Parser> parser, string source);
}
