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
            Assert.That(func.Name, Is.EqualTo("main"));
            Assert.That(((SingleValueTypeRef)func.ReturnType).Name, Is.EqualTo("int"));
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
            Assert.That(func.Name, Is.EqualTo("main"));
            
            Assert.That(func.ReturnType, Is.TypeOf<SingleValueTypeRef>());
            Assert.That(((SingleValueTypeRef)func.ReturnType).Name, Is.EqualTo("int"));
            
            Assert.That(func.Parameters, Has.Count.EqualTo(3));
            var p = func.Parameters.ToList();

            Assert.That(((SingleValueTypeRef)p[0].TypeRef).Name, Is.EqualTo("int"));
            Assert.That(((SingleValueTypeRef)p[1].TypeRef).Name, Is.EqualTo("float"));
            Assert.That(((SingleValueTypeRef)p[2].TypeRef).Name, Is.EqualTo("bool"));
        });
    }
}