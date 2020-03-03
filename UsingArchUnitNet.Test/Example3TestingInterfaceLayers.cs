using NUnit.Framework;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

namespace UsingArchUnitNet.Test
{
    public class Example3TestingInterfaceLayers
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(Class1).Assembly)
            .Build();

        private readonly IObjectProvider<IType> InterfaceLayer = ArchRuleDefinition.Types().That().ResideInNamespace("Contracts");
        private readonly IObjectProvider<Interface> Interfaces = ArchRuleDefinition.Interfaces();

        [Test]
        public void All_Interfaces_Are_In_Contracts_Namespace()
        {
            IArchRule interfacesShouldBeInContractsLayer =
                   ArchRuleDefinition.Interfaces().That().Are(Interfaces).Should().Be(InterfaceLayer);

            Assert.IsTrue(interfacesShouldBeInContractsLayer.HasNoViolations(Architecture));
        }
    }
}