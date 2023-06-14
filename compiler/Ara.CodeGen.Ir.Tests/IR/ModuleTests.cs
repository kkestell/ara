#region

using System.Collections.Generic;
using Ara.CodeGen.Ir.IR;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Tests.IR;

public class ModuleTests : TestBase
{
    [Test]
    public void MultipleFunctionDeclarations()
    {
        Module = new Module();

        var type1 = new FunctionType(IrType.Integer);
        var function1 = Module.AddFunction("test1", type1);

        var type2 = new FunctionType(IrType.Void);
        var function2 = Module.AddFunction("test2", type2);

        Builder = function1.IrBuilder();
        Builder = function2.IrBuilder();

        var ir = Module.Emit();
        AssertIr(ir, @"
            define i32 @test1 () {
            entry:
            }
            define void @test2 () {
            entry:
            }
        ");
    }

    [Test]
    public void FunctionWithParameters()
    {
        Module = new Module();

        var paramList = new List<Parameter> { new ("param1", IrType.Integer), new ("param2", IrType.Float) };
        var type = new FunctionType(IrType.Void, paramList);
        var function = Module.AddFunction("test", type);
        
        Builder = function.IrBuilder();

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test (i32 %param1, float %param2) {
            entry:
            }
        ");
    }

    [Test]
    public void DeclareFunctionTest()
    {
        Module = new Module();

        var funcDecl = new FunctionDeclaration("declaredFunc", IrType.Void, new List<IrType>() { IrType.Integer, IrType.Bool });
        Module.DeclareFunction(funcDecl);

        var ir = Module.Emit();
        AssertIr(ir, @"
            declare void @declaredFunc(i32, i1)
        ");
    }
    
    [Test]
    public void FunctionDefinitions()
    {
        Module = new Module();

        var type = new FunctionType(IrType.Integer);
        var function = Module.AddFunction("test", type);
        
        Builder = function.IrBuilder();

        var ir = Module.Emit();
        AssertIr(ir, @"
            define i32 @test () {
            entry:
            }
        ");
    }
    
    [Test]
    public void DeclareExternalFunctionTest()
    {
        Module = new Module();

        var externalFuncDecl = new ExternalFunctionDeclaration("externalFunc", IrType.Void, new List<IrType>() { IrType.Float, IrType.Integer });
        Module.DeclareExternalFunction(externalFuncDecl);

        var ir = Module.Emit();
        AssertIr(ir, @"
            declare void @externalFunc(float, i32)
        ");
    }
    
    [Test]
    public void MixedFunctionTypesTest()
    {
        Module = new Module();

        var type1 = new FunctionType(IrType.Integer);
        var function1 = Module.AddFunction("test1", type1);

        var funcDecl = new FunctionDeclaration("declaredFunc", IrType.Void, new List<IrType>() { IrType.Integer, IrType.Bool });
        Module.DeclareFunction(funcDecl);

        var externalFuncDecl = new ExternalFunctionDeclaration("externalFunc", IrType.Void, new List<IrType>() { IrType.Float, IrType.Integer });
        Module.DeclareExternalFunction(externalFuncDecl);

        var ir = Module.Emit();

        AssertIr(ir, @"
            declare void @externalFunc(float, i32)
            define i32 @test1 () {
            entry:
            }
            declare void @declaredFunc(i32, i1)
        ");
    }

}