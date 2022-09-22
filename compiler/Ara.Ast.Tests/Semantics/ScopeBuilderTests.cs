using Ara.Ast.Nodes;
using Ara.Ast.Semantics;

namespace Ara.Ast.Tests.Semantics;

public class ScopeBuilderTests : TestBase
{
    [Test]
    public void BuildsScopeForFunctionParameters()
    {
        using var tree = Parse(@"
            module main

            int sum(a: int, b: int) {
              return a + b
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder().Visit(ast);

        var block = ((FunctionDefinition)ast.Definitions.First()).Block;

        Assert.That(block.Scope.ContainsKey("a"), Is.True);
        Assert.That(block.Scope.ContainsKey("b"), Is.True);
    }
    
    [Test]
    public void BuildsScopeForLoopCounters()
    {
        using var tree = Parse(@"
            module main

            int sum(a: int, b: int) {
              int c = 0
              for i in 1..10 {
                c = c + i
              }
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder().Visit(ast);

        var block = ((For)((FunctionDefinition)ast.Definitions.First()).Block.Statements.Skip(1).First()).Block;
        Assert.That(block.Scope.ContainsKey("i"), Is.True);
    }
    
    [Test]
    public void BuildsScopeForVariableDeclarations()
    {
        using var tree = Parse(@"
            module main

            int sum() {
              int a = 0
              return a
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder().Visit(ast);

        var block = ((FunctionDefinition)ast.Definitions.First()).Block;
        Assert.That(block.Scope.ContainsKey("a"), Is.True);
    }
}