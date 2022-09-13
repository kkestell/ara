using Ara.Ast.Nodes.Expressions;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public record ReturnStatement(Node Node, Expression Expression) : Statement(Node);
