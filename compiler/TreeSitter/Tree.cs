using System.Runtime.InteropServices;

namespace Ara.TreeSitter;

public sealed class Tree : IDisposable
{
    readonly Handle<Tree> handle;
    readonly string source;

    public Tree(Handle<Tree> handle, string source)
    {
        this.handle = handle;
        this.source = source;
    }

    public Node Root => new(ts_tree_root_node(handle), this);

    public void Dispose()
    {
        delete_tree(handle);
    }

    public ReadOnlySpan<char> Span(int startByte, int endByte)
    {
        return source.AsSpan().Slice(startByte, endByte - startByte);
    }

    [DllImport("parser.so")]
    static extern TsNode ts_tree_root_node(Handle<Tree> handle);

    [DllImport("parser.so")]
    static extern void delete_tree(Handle<Tree> handle);
}