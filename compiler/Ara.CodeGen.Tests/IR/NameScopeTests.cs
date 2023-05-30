#region

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Ara.CodeGen.IR;

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
        Assert.AreEqual("Name cannot be empty", exception.Message);
    }

    [Test]
    public void Register_LongSequenceOfDuplicates()
    {
        var nameScope = new NameScope();
        var baseName = "TestName";

        for (int i = 0; i < 10000; i++)
        {
            var name = nameScope.Register(baseName);
            Assert.AreEqual($"{baseName}.{i}", name);
            Assert.IsTrue(nameScope.Names.Contains(name));
        }
    }

    [Test]
    public void Register_Multithreading()
    {
        var nameScope = new NameScope();
        var tasks = new Task[100];
        var registeredNames = new ConcurrentBag<string>();

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < 100; j++)
                {
                    var name = nameScope.Register("TestName");
                    registeredNames.Add(name);
                }
            });
        }

        Task.WaitAll(tasks);
    
        foreach (var name in registeredNames)
        {
            Assert.IsTrue(nameScope.Names.Contains(name));
        }
    }
}
