#region

using System.Runtime.InteropServices;

#endregion

namespace Ara.Parsing;

public partial class Parser : IDisposable
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

    [LibraryImport(Platform.SharedLibrary, EntryPoint = "create_parser")]
    private static partial Handle<Parser> CreateParser();

    [LibraryImport(Platform.SharedLibrary, EntryPoint = "delete_parser")]
    private static partial void DeleteParser(Handle<Parser> parser);

    [LibraryImport(Platform.SharedLibrary, EntryPoint = "parse", StringMarshalling = StringMarshalling.Utf8)]
    private static partial Handle<Tree> Parse(Handle<Parser> parser, string source);
}
