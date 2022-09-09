using Ara.Ast.Nodes.Expressions;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public record IfStatement(Node Node, Expression Predicate, Block Then) : Statement(Node);