using Ara.Ast;
using Ara.Ast.Nodes;

namespace Ara.Tests.Ast;

public class Tests : TestBase
{
    [Test]
    public void TransformFunction()
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
}