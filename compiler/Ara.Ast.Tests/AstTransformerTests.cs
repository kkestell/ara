using Ara.Ast.Nodes;

namespace Ara.Ast.Tests;

public class AstTransformerTests : TestBase
{
    [Test]
    public void TransformSimpleFunction()
    {
        using var tree = Parse(@"
            module main

            fn main() -> int {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        
        var func = (FunctionDefinition)ast.Definitions.Nodes.First();
        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("main"));
            Assert.That(((SingleValueTypeRef)func.ReturnType).Name, Is.EqualTo("int"));
            Assert.That(func.Parameters.Nodes, Is.Empty);
        });
    }
    
    [Test]
    public void TransformFunctionWithParameters()
    {
        using var tree = Parse(@"
            module main

            fn main(a: int, b: float, c: bool) -> int {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        
        var func = (FunctionDefinition)ast.Definitions.Nodes.First();
        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("main"));
            
            Assert.That(func.ReturnType, Is.TypeOf<SingleValueTypeRef>());
            Assert.That(((SingleValueTypeRef)func.ReturnType).Name, Is.EqualTo("int"));
            
            Assert.That(func.Parameters.Nodes.Count(), Is.EqualTo(3));
            var p = func.Parameters.Nodes.ToList();

            Assert.That(((SingleValueTypeRef)p[0].TypeRef).Name, Is.EqualTo("int"));
            Assert.That(((SingleValueTypeRef)p[1].TypeRef).Name, Is.EqualTo("float"));
            Assert.That(((SingleValueTypeRef)p[2].TypeRef).Name, Is.EqualTo("bool"));
        });
    }
}