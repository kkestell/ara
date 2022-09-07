using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public record VariableDeclarationStatement(Node Node, Identifier Name, Type_ Type, Expression Expression) : Statement(Node);
