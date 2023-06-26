#region

using System;
using System.Linq;
using Ara.CodeGen.Ir.IR;

#endregion

namespace Ara.CodeGen.Tests.IR;

public class NameScopeTests
{
    [Test]
    public void Register_DuplicateName()
    {
        var nameScope = new NameScope();
        var name1 = nameScope.Register("TestName");
        var name2 = nameScope.Register("TestName");
    
        Assert.AreEqual("TestName", name1);
        Assert.AreEqual("TestName.1", name2);
        Assert.IsTrue(nameScope.Names.Contains(name1));
        Assert.IsTrue(nameScope.Names.Contains(name2));
    }

    [Test]
    public void Register_EmptyString()
    {
        var nameScope = new NameScope();
        var exception = Assert.Throws<ArgumentException>(() => nameScope.Register(string.Empty));
        Assert.AreEqual("Name cannot be empty", exception!.Message);
    }
}
