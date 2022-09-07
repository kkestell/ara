using Ara.Ast.Nodes.Statements;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record StatementList(Node Node, IEnumerable<Statement> Values) : AstNode(Node);
