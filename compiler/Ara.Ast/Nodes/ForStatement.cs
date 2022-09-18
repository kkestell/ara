using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ForStatement(Node Node, Identifier Counter, Expression Start, Expression End, Block Block) : Statement(Node);
