using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record TypeRef(Node Node) : AstNode(Node);

public record SingleValueTypeRef(Node Node, Identifier Name) : TypeRef(Node);

public record ArrayTypeRef(Node Node, TypeRef Type, Constant Size) : TypeRef(Node);