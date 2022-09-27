using System.Runtime.InteropServices;

namespace Ara.Parsing;

public sealed class Node
{
    readonly TsNode handle;

    public Node(TsNode handle, Tree tree)
    {
        this.handle = handle;
        Tree = tree;
    }

    public Tree Tree { get; }

    public Location Location => new (this);

    public Tuple<int, int> Offset => 
        new (TsNodeStartByte(handle), TsNodeEndByte(handle));

    public TsPoint StartPoint => TsNodeStartPoint(handle);
    
    public TsPoint EndPoint => TsNodeEndPoint(handle);

    public ReadOnlySpan<char> Span => 
        Tree.AsSpan(TsNodeStartByte(handle), TsNodeEndByte(handle));

    public string Type => 
        Marshal.PtrToStringUTF8(TsNodeType(handle))!;

    public Node? Parent => 
        OptionalNode(TsNodeParent(handle));

    public Node? NextNamedSibling => 
        OptionalNode(TsNodeNextNamedSibling(handle));

    public Node? PreviousNamedSibling => 
        OptionalNode(TsNodePrevNamedSibling(handle));

    public int ChildCount => 
        TsNodeChildCount(handle);

    public int NamedChildCount => 
        TsNodeNamedChildCount(handle);

    public Node? ChildByFieldName(string fieldName) =>
        OptionalNode(TsNodeChildByFieldName(handle, fieldName, fieldName.Length));

    public string Sexp() => 
        TsNodeString(handle);

    public IEnumerable<Node> Children
    {
        get
        {
            for (var i = 0; i < ChildCount; i++)
                yield return new Node(TsNodeChild(handle, i), Tree);
        }
    }

    public IEnumerable<Node> NamedChildren
    {
        get
        {
            for (var i = 0; i < NamedChildCount; i++)
                yield return new Node(TsNodeNamedChild(handle, i), Tree);
        }
    }

    Node? OptionalNode(TsNode node) =>
        TsNodeIsNull(node) ? null : new Node(node, Tree);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_type")]
    static extern IntPtr TsNodeType(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_parent")]
    static extern TsNode TsNodeParent(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_child_count")]
    static extern int TsNodeChildCount(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_named_child_count")]
    static extern int TsNodeNamedChildCount(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_child")]
    static extern TsNode TsNodeChild(TsNode node, int child);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_named_child")]
    static extern TsNode TsNodeNamedChild(TsNode node, int child);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_is_null")]
    [return: MarshalAs(UnmanagedType.I1)]
    static extern bool TsNodeIsNull(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_next_named_sibling")]
    static extern TsNode TsNodeNextNamedSibling(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_prev_named_sibling")]
    static extern TsNode TsNodePrevNamedSibling(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_start_byte")]
    static extern int TsNodeStartByte(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_end_byte")]
    static extern int TsNodeEndByte(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_start_point")]
    static extern TsPoint TsNodeStartPoint(TsNode node);
    
    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_end_point")]
    static extern TsPoint TsNodeEndPoint(TsNode node);
    
    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_string", CharSet = CharSet.Ansi)]
    static extern string TsNodeString(TsNode node);
    
    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_child_by_field_name", CharSet = CharSet.Ansi)]
    static extern TsNode TsNodeChildByFieldName(TsNode node, [MarshalAs(UnmanagedType.LPStr)] string fieldName, int fieldNameLength);
}