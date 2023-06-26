#region

using Ara.Ast.Errors;
using Ara.Ast.Semantics;

#endregion

namespace Ara.Ast.Tests.Semantics;

public class TypeCheckerTests : TestBase
{
    [Test]
    public void ThrowWhenReturnTypeIsInvalid()
    {
        using var tree = Parse(@"
            fn main() -> int {
              return 1.5
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        Assert.Throws<ReturnTypeException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenTooManyArguments()
    {
        using var tree = Parse(@"
            fn test(a: int) -> int {
              return 0
            }

            fn main() -> int {
              return test(1, 2)
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenTooFewArguments()
    {
        using var tree = Parse(@"
            fn test(a: int) -> int {
              return 0
            }

            fn main() -> int {
              return test()
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenArgumentTypeInvalid()
    {
        using var tree = Parse(@"
            fn test(a: int) -> int {
              return 0
            }

            fn main() -> int {
              return test(1.5)
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenIfPredicateTypeIsInvalid()
    {
        using var tree = Parse(@"
            fn main() -> int {
              if 1 {
                return 1
              }
              return 0
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        Assert.Throws<PredicateTypeException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
}