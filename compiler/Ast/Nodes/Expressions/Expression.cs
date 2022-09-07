using Ara.Parsing;

namespace Ara.Ast.Nodes.Expressions;

public abstract record Expression(Node Node) : AstNode(Node);
