using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record ArrayTypeRef(Node Node, TypeRef Type, Constant Size) : TypeRef(Node);