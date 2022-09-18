using Ara.Ast.Nodes;

namespace Ara.Ast.Tests;

public class AstTransformerTests : TestBase
{
    [Test]
    public void TransformSimpleFunction()
    {
        using var tree = Parse(@"
            module main

            int main() {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        
        var func = (FunctionDefinition)ast.Definitions.First();
        Assert.Multiple(() =>
        {
            Assert.That(func.Name.Value, Is.EqualTo("main"));
            Assert.That(func.ReturnType.Value, Is.EqualTo("int"));
            Assert.That(func.Parameters, Is.Empty);
        });
    }
    
    [Test]
    public void TransformFunctionWithParameters()
    {
        using var tree = Parse(@"
            module main

            int main(a: int, b: float, c: bool) {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        
        var func = (FunctionDefinition)ast.Definitions.First();
        Assert.Multiple(() =>
        {
            Assert.That(func.Name.Value, Is.EqualTo("main"));
            Assert.That(func.ReturnType.Value, Is.EqualTo("int"));
            Assert.That(func.Parameters, Has.Count.EqualTo(3));
            var p = func.Parameters.ToList();
            Assert.That(func.Parameters[0].Name.Value, Is.EqualTo("a"));
            Assert.That(func.Parameters[0].Type.Value, Is.EqualTo("int"));
            Assert.That(func.Parameters[1].Name.Value, Is.EqualTo("b"));
            Assert.That(func.Parameters[1].Type.Value, Is.EqualTo("float"));
            Assert.That(func.Parameters[2].Name.Value, Is.EqualTo("c"));
            Assert.That(func.Parameters[2].Type.Value, Is.EqualTo("bool"));
        });
    }
}