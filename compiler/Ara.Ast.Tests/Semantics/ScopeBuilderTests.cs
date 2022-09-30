using Ara.Ast.Nodes;
using Ara.Ast.Semantics;

namespace Ara.Ast.Tests.Semantics;

public class ScopeBuilderTests : TestBase
{
    [Test]
    public void BuildsScopeForFunctionParameters()
    {
        using var tree = Parse(@"
            fn sum(a: int, b: int) -> int {
              return a + b
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        var block = ((FunctionDefinition)ast.Definitions.Nodes.First()).Block;

        Assert.That(block.Scope.ContainsKey("a"), Is.True);
        Assert.That(block.Scope.ContainsKey("b"), Is.True);
    }
    
    [Test]
    public void BuildsScopeForLoopCounters()
    {
        using var tree = Parse(@"
            fn sum(a: int, b: int) -> int {
              var c: int = 0
              for i in 1..10 {
                c = c + i
              }
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        var block = ((For)((FunctionDefinition)ast.Definitions.Nodes.First()).Block.Statements.Nodes[1]).Block;
        Assert.That(block.Scope.ContainsKey("i"), Is.True);
    }
    
    [Test]
    public void BuildsScopeForVariableDeclarations()
    {
        using var tree = Parse(@"
            fn sum() -> int {
              var a: int = 0
              return a
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();

        var block = ((FunctionDefinition)ast.Definitions.Nodes.First()).Block;
        Assert.That(block.Scope.ContainsKey("a"), Is.True);
    }
}