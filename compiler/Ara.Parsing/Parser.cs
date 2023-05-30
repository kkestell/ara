#region

using System.Runtime.InteropServices;

#endregion

namespace Ara.Parsing;

public sealed class Parser : IDisposable
{
    private readonly Handle<Parser> _handle;

    public Parser()
    {
        _handle = CreateParser();
    }

    public void Dispose()
    {
        DeleteParser(_handle);
    }

    public Tree Parse(string source, string? filename = null)
    {
        var tree = Parse(_handle, source);
        return new Tree(tree, source, filename);
    }

    [DllImport(Platform.SharedLibrary, EntryPoint = "create_parser")]
    private static extern Handle<Parser> CreateParser();

    [DllImport(Platform.SharedLibrary, EntryPoint = "delete_parser")]
    private static extern void DeleteParser(Handle<Parser> parser);

    [DllImport(Platform.SharedLibrary, EntryPoint = "parse", CharSet = CharSet.Ansi)]
    private static extern Handle<Tree> Parse(Handle<Parser> parser, string source);
}
