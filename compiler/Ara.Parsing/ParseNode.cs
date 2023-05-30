#region

using System.Runtime.InteropServices;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Parsing;

public sealed class ParseNode : IParseNode
{
    private readonly TsNode _handle;

    public ParseNode(TsNode handle, Tree tree)
    {
        _handle = handle;
        Tree = tree;
    }

    public Tree Tree { get; }

    public Location Location => new (this);

    public Tuple<int, int> Offset => 
        new (TsNodeStartByte(_handle), TsNodeEndByte(_handle));

    public TsPoint StartPoint => TsNodeStartPoint(_handle);
    
    public TsPoint EndPoint => TsNodeEndPoint(_handle);

    public ReadOnlySpan<char> Span => 
        Tree.AsSpan(TsNodeStartByte(_handle), TsNodeEndByte(_handle));

    public string Type => 
        Marshal.PtrToStringUTF8(TsNodeType(_handle))!;

    public ParseNode? Parent => 
        OptionalNode(TsNodeParent(_handle));

    public ParseNode? NextNamedSibling => 
        OptionalNode(TsNodeNextNamedSibling(_handle));

    public ParseNode? PreviousNamedSibling => 
        OptionalNode(TsNodePrevNamedSibling(_handle));

    public int ChildCount => 
        TsNodeChildCount(_handle);

    public int NamedChildCount => 
        TsNodeNamedChildCount(_handle);

    public ParseNode? ChildByFieldName(string fieldName) =>
        OptionalNode(TsNodeChildByFieldName(_handle, fieldName, fieldName.Length));

    public string Sexp() => 
        TsNodeString(_handle);

    public IEnumerable<ParseNode> Children
    {
        get
        {
            for (var i = 0; i < ChildCount; i++)
                yield return new ParseNode(TsNodeChild(_handle, i), Tree);
        }
    }

    public IEnumerable<ParseNode> NamedChildren
    {
        get
        {
            for (var i = 0; i < NamedChildCount; i++)
                yield return new ParseNode(TsNodeNamedChild(_handle, i), Tree);
        }
    }

    private ParseNode? OptionalNode(TsNode node) =>
        TsNodeIsNull(node) ? null : new ParseNode(node, Tree);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_type")]
    private static extern IntPtr TsNodeType(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_parent")]
    private static extern TsNode TsNodeParent(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_child_count")]
    private static extern int TsNodeChildCount(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_named_child_count")]
    private static extern int TsNodeNamedChildCount(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_child")]
    private static extern TsNode TsNodeChild(TsNode node, int child);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_named_child")]
    private static extern TsNode TsNodeNamedChild(TsNode node, int child);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_is_null")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool TsNodeIsNull(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_next_named_sibling")]
    private static extern TsNode TsNodeNextNamedSibling(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_prev_named_sibling")]
    private static extern TsNode TsNodePrevNamedSibling(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_start_byte")]
    private static extern int TsNodeStartByte(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_end_byte")]
    private static extern int TsNodeEndByte(TsNode node);

    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_start_point")]
    private static extern TsPoint TsNodeStartPoint(TsNode node);
    
    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_end_point")]
    private static extern TsPoint TsNodeEndPoint(TsNode node);
    
    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_string", CharSet = CharSet.Ansi)]
    private static extern string TsNodeString(TsNode node);
    
    [DllImport(Platform.SharedLibrary, EntryPoint = "ts_node_child_by_field_name", CharSet = CharSet.Ansi)]
    private static extern TsNode TsNodeChildByFieldName(TsNode node, [MarshalAs(UnmanagedType.LPStr)] string fieldName, int fieldNameLength);
}