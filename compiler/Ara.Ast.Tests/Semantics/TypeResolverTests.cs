using Ara.Ast.Nodes;
using Ara.Ast.Semantics;

namespace Ara.Ast.Tests.Semantics;

public class TypeResolverTests : TestBase
{
    [Test]
    public void InfersTypeOfExpressions()
    {
        using var tree = Parse(@"
            module main

            int sum(a: int, b: int) {
              return a + b
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder().Visit(ast);
        new TypeResolver().Visit(ast);

        var block = ((FunctionDefinition)ast.Definitions.First()).Block;
        var ret = (Return)block.Statements.First();

        Assert.That(ret.Expression.InferredType, Is.Not.Null);
        Assert.That(ret.Expression.InferredType!.Value, Is.EqualTo("int"));
    }
}