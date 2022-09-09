using Ara.Ast.Nodes.Statements;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Block(Node Node, IEnumerable<Statement> Statements) : AstNode(Node);
