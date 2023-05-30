#region

using Ara.Ast.Nodes;

#endregion

namespace Ara.Ast.Tests;

public class AstTransformerTests : TestBase
{
    [Test]
    public void TransformSimpleFunction()
    {
        using var tree = Parse(@"
            fn main() {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        
        var func = ast.FunctionDefinitions.Nodes.First();
        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("main"));
            Assert.That(func.Parameters.Nodes, Is.Empty);
        });
    }
    
    [Test]
    public void TransformFunctionWithParameters()
    {
        using var tree = Parse(@"
            fn main(a: int, b: float, c: bool) -> int {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        
        var func = ast.FunctionDefinitions.Nodes.First();
        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("main"));

            Assert.That(func.Parameters.Nodes, Has.Count.EqualTo(3));
            var p = func.Parameters.Nodes.ToList();

            Assert.That(((SingleValueTypeRef)p[0].TypeRef).Name, Is.EqualTo("int"));
            Assert.That(((SingleValueTypeRef)p[1].TypeRef).Name, Is.EqualTo("float"));
            Assert.That(((SingleValueTypeRef)p[2].TypeRef).Name, Is.EqualTo("bool"));
        });
    }
}