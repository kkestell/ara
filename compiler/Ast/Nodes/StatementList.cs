using Ara.Ast.Nodes.Statements;
using Ara.TreeSitter;

namespace Ara.Ast.Nodes;

public record StatementList(Node Node, IEnumerable<Statement> Values) : AstNode(Node);
