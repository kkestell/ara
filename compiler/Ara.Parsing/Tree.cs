#region

using System.Runtime.InteropServices;

#endregion

namespace Ara.Parsing;

public sealed class Tree : IDisposable
{
    private readonly Handle<Tree> _handle;
    private readonly string _source;

    public Tree(Handle<Tree> handle, string source, string? filename = null)
    {
        _handle = handle;
        _source = source;
        Filename = filename;
    }

    public string? Filename { get; }

    public ParseNode Root => new(TsTreeRootNode(_handle), this);

    public void Dispose()
    {
        DeleteTree(_handle);
    }

    public ReadOnlySpan<char> AsSpan(int startByte, int endByte)
    {
        return _source.AsSpan().Slice(startByte, endByte - startByte);
    }

    public ReadOnlySpan<char> AsSpan()
    {
        return _source.AsSpan();
    }

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_tree_root_node")]
    private static extern TsNode TsTreeRootNode(Handle<Tree> handle);

    [DllImport(Platform.SharedLibrary, EntryPoint = "delete_tree")]
    private static extern void DeleteTree(Handle<Tree> handle);
}
