using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public abstract record Statement(Node node) : AstNode(node);
