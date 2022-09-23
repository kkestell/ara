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
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        
        Assert.Throws<ReturnTypeException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenTooManyArguments()
    {
        using var tree = Parse($@"
            module main

            void test(a: int) {{
              return 0
            }}

            int main() {{
              return test(a: 1, b: 2)
            }}
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        
        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenTooFewArguments()
    {
        using var tree = Parse($@"
            module main

            void test(a: int) {{
              return 0
            }}

            int main() {{
              return test()
            }}
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        
        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenArgumentNameInvalid()
    {
        using var tree = Parse($@"
            module main

            void test(a: int) {{
              return 0
            }}

            int main() {{
              return test(b: 1)
            }}
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        
        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenArgumentTypeInvalid()
    {
        using var tree = Parse($@"
            module main

            void test(a: int) {{
              return 0
            }}

            int main() {{
              return test(a: 1.5)
            }}
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        
        Assert.Throws<SemanticException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
    
    [Test]
    public void ThrowWhenIfPredicateTypeIsInvalid()
    {
        using var tree = Parse($@"
            module main

            int main() {{
              if 1 {{
                return 1
              }}
              return 0
            }}
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeResolver(ast).Visit();
        
        Assert.Throws<IfPredicateTypeException>(delegate
        {
            new TypeChecker(ast).Visit();
        });
    }
}