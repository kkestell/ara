using System.Runtime.InteropServices;

namespace Ara.Parsing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TsNode
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    readonly uint[] Context;

    readonly IntPtr Id;
    readonly IntPtr Tree;
}

public sealed class Node
{
    const string SharedLibrary = "parser.so";

    readonly TsNode handle;
    readonly Tree tree;

    public Node(TsNode handle, Tree tree)
    {
        this.handle = handle;
        this.tree = tree;
    }

    public ReadOnlySpan<char> Span => 
        tree.Span(ts_node_start_byte(handle), ts_node_end_byte(handle));

    public string Type => 
        Marshal.PtrToStringAuto(ts_node_type(handle))!;

    public Node? Parent => 
        OptionalNode(ts_node_parent(handle));

    public Node? NextNamedSibling => 
        OptionalNode(ts_node_next_named_sibling(handle));

    public Node? PreviousNamedSibling => 
        OptionalNode(ts_node_prev_named_sibling(handle));

    public int ChildCount => 
        ts_node_child_count(handle);

    public int NamedChildCount => 
        ts_node_named_child_count(handle);

    public Node? ChildByFieldName(string fieldName) =>
        OptionalNode(ts_node_child_by_field_name(handle, fieldName, fieldName.Length));

    public string Sexp() => 
        ts_node_string(handle);

    public IEnumerable<Node> Children
    {
        get
        {
            for (var i = 0; i < ChildCount; i++)
                yield return new Node(ts_node_child(handle, i), tree);
        }
    }

    public IEnumerable<Node> NamedChildren
    {
        get
        {
            for (var i = 0; i < NamedChildCount; i++)
                yield return new Node(ts_node_named_child(handle, i), tree);
        }
    }

    Node? OptionalNode(TsNode node) =>
        ts_node_is_null(node) ? null : new Node(node, tree);

    [DllImport(SharedLibrary)]
    static extern IntPtr ts_node_type(TsNode node);

    [DllImport(SharedLibrary)]
    static extern TsNode ts_node_parent(TsNode node);

    [DllImport(SharedLibrary)]
    static extern int ts_node_child_count(TsNode node);

    [DllImport(SharedLibrary)]
    static extern int ts_node_named_child_count(TsNode node);

    [DllImport(SharedLibrary)]
    static extern TsNode ts_node_child(TsNode node, int child);

    [DllImport(SharedLibrary)]
    static extern TsNode ts_node_named_child(TsNode node, int child);

    [DllImport(SharedLibrary)]
    [return: MarshalAs(UnmanagedType.I1)]
    static extern bool ts_node_is_null(TsNode node);

    [DllImport(SharedLibrary)]
    static extern TsNode ts_node_next_named_sibling(TsNode node);

    [DllImport(SharedLibrary)]
    static extern TsNode ts_node_prev_named_sibling(TsNode node);

    [DllImport(SharedLibrary)]
    static extern int ts_node_start_byte(TsNode node);

    [DllImport(SharedLibrary)]
    static extern int ts_node_end_byte(TsNode node);
    
    [DllImport(SharedLibrary)]
    static extern string ts_node_string(TsNode node);
    
    [DllImport(SharedLibrary)]
    static extern TsNode ts_node_child_by_field_name(TsNode node, string fieldName, int fieldNameLength);
}