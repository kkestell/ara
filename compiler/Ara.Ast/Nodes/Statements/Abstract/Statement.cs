using Ara.Ast.Nodes.Abstract;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements.Abstract;

public abstract record Statement(Node Node) : AstNode(Node);
