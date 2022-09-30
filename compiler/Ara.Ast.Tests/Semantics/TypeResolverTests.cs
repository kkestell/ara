using Ara.Ast.Nodes;
using Ara.Ast.Semantics;
using Ara.Ast.Semantics.Types;

namespace Ara.Ast.Tests.Semantics;

public class TypeResolverTests : TestBase
{
    [Test]
    public void InfersTypeOfExpressions()
    {
        using var tree = Parse(@"
            fn sum(a: int, b: int) -> int {
              return a + b
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();

        var block = (ast.FunctionDefinitions.Nodes.First()).Block;
        var ret = (Return)block.Statements.Nodes[0];

        Assert.That(ret.Expression.Type, Is.Not.Null);
        Assert.That(ret.Expression.Type, Is.TypeOf<IntegerType>());
    }
}