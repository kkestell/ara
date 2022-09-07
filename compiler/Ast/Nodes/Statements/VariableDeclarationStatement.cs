using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.TreeSitter;

namespace Ara.Ast.Nodes.Statements;

public record VariableDeclarationStatement(Node Node, Identifier Name, TypeNode Type, Expression Expression) : Statement(Node);
