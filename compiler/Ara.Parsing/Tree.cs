using System.Runtime.InteropServices;

namespace Ara.Parsing;

public sealed class Tree : IDisposable
{
    const string SharedLibrary = "AraParser.dll";

    readonly Handle<Tree> handle;
    readonly string source;

    public Tree(Handle<Tree> handle, string source, string? filename = null)
    {
        this.handle = handle;
        this.source = source;
        Filename = filename;
    }

    public string? Filename { get; }

    public Node Root => new(ts_tree_root_node(handle), this);

    public void Dispose()
    {
        delete_tree(handle);
    }

    public ReadOnlySpan<char> AsSpan(int startByte, int endByte)
    {
        return source.AsSpan().Slice(startByte, endByte - startByte);
    }

    public ReadOnlySpan<char> AsSpan()
    {
        return source.AsSpan();
    }

    [DllImport(SharedLibrary)]
    static extern TsNode ts_tree_root_node(Handle<Tree> handle);

    [DllImport(SharedLibrary)]
    static extern void delete_tree(Handle<Tree> handle);
}