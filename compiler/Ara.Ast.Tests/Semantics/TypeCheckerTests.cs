using Ara.Ast.Errors;
using Ara.Ast.Semantics;

namespace Ara.Ast.Tests.Semantics;

public class TypeCheckerTests : TestBase
{
    [Test]
    public void ThrowWhenReturnTypeIsInvalid()
    {
        using var tree = Parse(@"
            module main

            int main() {
              return 1.5
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder().Visit(ast);
        new TypeResolver().Visit(ast);
        
        Assert.Throws<ReturnTypeException>(delegate
        {
            new TypeChecker().Visit(ast);
        });
    }
}