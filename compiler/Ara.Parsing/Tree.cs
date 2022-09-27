using System.Runtime.InteropServices;

namespace Ara.Parsing;

public sealed class Tree : IDisposable
{
    readonly Handle<Tree> handle;
    readonly string source;

    public Tree(Handle<Tree> handle, string source, string? filename = null)
    {
        this.handle = handle;
        this.source = source;
        Filename = filename;
    }

    public string? Filename { get; }

    public Node Root => new(TsTreeRootNode(handle), this);

    public void Dispose()
    {
        DeleteTree(handle);
    }

    public ReadOnlySpan<char> AsSpan(int startByte, int endByte)
    {
        return source.AsSpan().Slice(startByte, endByte - startByte);
    }

    public ReadOnlySpan<char> AsSpan()
    {
        return source.AsSpan();
    }

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_tree_root_node")]
    static extern TsNode TsTreeRootNode(Handle<Tree> handle);

    [DllImport(Platform.SharedLibrary, EntryPoint = "delete_tree")]
    static extern void DeleteTree(Handle<Tree> handle);
}