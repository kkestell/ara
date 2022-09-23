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
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();

        var block = ((FunctionDefinition)ast.Definitions.First()).Block;
        var ret = (Return)block.Statements.First();

        Assert.That(ret.Expression.Type, Is.Not.Null);
        Assert.That(ret.Expression.Type, Is.TypeOf<IntegerType>());
    }
}