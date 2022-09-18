using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record For(Node Node, Identifier Counter, Expression Start, Expression End, Block Block) : Statement(Node);
