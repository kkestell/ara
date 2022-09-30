using Ara.Ast.Errors;
using Ara.Ast.Semantics;

namespace Ara.Ast.Tests.Semantics;

public class ArrayBoundsCheckerTests : TestBase
{
    [Test]
    public void ThrowWhenArrayIsOutOfBounds()
    {
        using var tree = Parse(@"
            fn main() -> int {
              a: int[3]
              return a[5]
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        new TypeChecker(ast).Visit();

        Assert.Throws<ArrayIndexOutOfBoundsException>(delegate
        {
            new ArrayBoundsChecker(ast).Visit();
        });
    }
    
    // FIXME: Move these guards, and tests, elsewhere...
    
    [Test]
    public void ThrowWhenIndexIsNotAnInteger()
    {
        using var tree = Parse(@"
            fn main() -> int {
              a: int[3]
              return a[true]
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        new TypeChecker(ast).Visit();

        Assert.Throws<SemanticException>(delegate
        {
            new ArrayBoundsChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenIndexIsNotAConstantValue()
    {
        using var tree = Parse(@"
            fn test() -> int {
              return 1
            }

            fn main() -> int {
              a: int[3]
              return a[test()]
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        new TypeChecker(ast).Visit();

        Assert.Throws<SemanticException>(delegate
        {
            new ArrayBoundsChecker(ast).Visit();
        });
    }
}