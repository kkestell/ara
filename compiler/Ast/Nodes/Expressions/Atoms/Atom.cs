using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Expressions.Atoms;

public abstract record Atom(Node Node) : Expression(Node);
