using NUnit.Framework;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using System.Linq;

namespace UsingArchUnitNet.Test
{
    public class Example2TestingInterfaceNameConvention
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(Class1).Assembly)
            .Build();

        [Test]
        public void All_Interfaces_Start_With_I()
        {
            Assert.IsTrue(Architecture.Interfaces.All(i => i.Name.StartsWith("I")));
        }
    }
}