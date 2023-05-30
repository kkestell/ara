#region

using Ara.Ast.Nodes.Abstract;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes.Statements.Abstract;

public abstract record Statement(IParseNode Node) : AstNode(Node);
