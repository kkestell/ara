using System;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public record Assignment(Node Node, Identifier Name, Expression Expression) : Statement(Node);
