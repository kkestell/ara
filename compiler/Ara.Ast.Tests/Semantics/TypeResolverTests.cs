using Ara.Ast.Errors;
using Ara.Ast.Semantics;

namespace Ara.Ast.Tests.Semantics;

public class TypeResolverTests : TestBase
{
    [Test]
    public void ThrowWhenReturningMoreThanOneType()
    {
        using var tree = Parse(@"
            fn main() {
              if true {
                return 1
              } else {
                return 1.5
              }
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        Assert.Throws<SemanticException>(delegate
        {
            new TypeResolver(ast).Visit();
        });
    }
}