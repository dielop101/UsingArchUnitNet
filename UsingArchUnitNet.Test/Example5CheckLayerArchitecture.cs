using NUnit.Framework;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Dummy.Application;
using Dummy.Controllers;
using Dummy.Domain;
using Dummy.Infrastructure;

namespace UsingArchUnitNet.Test
{
    public class Example5CheckLayerArchitecture
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(
                typeof(Controller).Assembly, 
                typeof(Application).Assembly,
                typeof(Domain).Assembly,
                typeof(Infrastructure).Assembly)
            .Build();

        private readonly IObjectProvider<IType> ControllerLayer =
            ArchRuleDefinition.Types().That().ResideInNamespace("Controllers").As("Controllers Layer");
        private readonly IObjectProvider<IType> ApplicationLayer =
            ArchRuleDefinition.Types().That().ResideInNamespace("Application").As("Application Layer");
        private readonly IObjectProvider<IType> DomainLayer =
            ArchRuleDefinition.Types().That().ResideInNamespace("Domain").As("Domain Layer");
        private readonly IObjectProvider<IType> InfrastructureLayer =
            ArchRuleDefinition.Types().That().ResideInNamespace("Infrastructure").As("Infrastructure Layer");

        [Test]
        public void Check_Controller_Dependencies()
        {
            IArchRule controllerLayerShouldNotAccessInfrastructureLayer =
                ArchRuleDefinition.Types().That().Are(ControllerLayer).Should().NotDependOnAny(InfrastructureLayer);

            IArchRule controllerLayerShouldNotAccessDomainLayer =
                ArchRuleDefinition.Types().That().Are(ControllerLayer).Should().NotDependOnAny(DomainLayer);

            IArchRule controllerLayerShouldAccessDomainLayer =
                ArchRuleDefinition.Types().That().Are(ControllerLayer).Should().DependOnAny(ApplicationLayer);

            IArchRule combinedArchRule =
                controllerLayerShouldNotAccessInfrastructureLayer
                    .And(controllerLayerShouldNotAccessDomainLayer)
                    .And(controllerLayerShouldAccessDomainLayer);

            Assert.IsTrue(combinedArchRule.HasNoViolations(Architecture));
        }

        [Test]
        public void Check_Application_Dependencies()
        {
            IArchRule applicationLayerShouldNotAccessInfrastructureLayer =
                ArchRuleDefinition.Types().That().Are(ApplicationLayer).Should().NotDependOnAny(InfrastructureLayer);

            IArchRule applicationLayerShouldAccessDomainLayer =
                ArchRuleDefinition.Types().That().Are(ApplicationLayer).Should().DependOnAny(DomainLayer);

            IArchRule applicationLayerShouldNotAccessControllerLayer =
                ArchRuleDefinition.Types().That().Are(ApplicationLayer).Should().NotDependOnAny(ControllerLayer);

            IArchRule combinedArchRule =
                applicationLayerShouldNotAccessInfrastructureLayer
                    .And(applicationLayerShouldNotAccessControllerLayer)
                    .And(applicationLayerShouldAccessDomainLayer);

            Assert.IsTrue(combinedArchRule.HasNoViolations(Architecture));
        }

        [Test]
        public void Check_Domain_Dependencies()
        {
            IArchRule domainLayerShouldNotAccessInfrastructureLayer =
                ArchRuleDefinition.Types().That().Are(DomainLayer).Should().NotDependOnAny(InfrastructureLayer);

            IArchRule applicationLayerShouldNotAccessApplicationLayer =
                ArchRuleDefinition.Types().That().Are(DomainLayer).Should().NotDependOnAny(ApplicationLayer);

            IArchRule applicationLayerShouldNotAccessControllerLayer =
                ArchRuleDefinition.Types().That().Are(DomainLayer).Should().NotDependOnAny(ControllerLayer);

            IArchRule combinedArchRule =
                domainLayerShouldNotAccessInfrastructureLayer
                    .And(applicationLayerShouldNotAccessApplicationLayer)
                    .And(applicationLayerShouldNotAccessControllerLayer);

            Assert.IsTrue(combinedArchRule.HasNoViolations(Architecture));
        }

        [Test]
        public void Check_Infrastructure_Dependencies()
        {
            IArchRule infrastructureLayerShouldNotAccessControllerLayer =
                ArchRuleDefinition.Types().That().Are(InfrastructureLayer).Should().NotDependOnAny(ControllerLayer);

            IArchRule infrastructureLayerShouldAccessDomainLayer =
                ArchRuleDefinition.Types().That().Are(InfrastructureLayer).Should().DependOnAny(DomainLayer);

            IArchRule infrastructureLayerShouldNotAccessApplicationLayer =
                ArchRuleDefinition.Types().That().Are(InfrastructureLayer).Should().NotDependOnAny(ApplicationLayer);

            IArchRule combinedArchRule =
                infrastructureLayerShouldNotAccessControllerLayer
                    .And(infrastructureLayerShouldAccessDomainLayer)
                    .And(infrastructureLayerShouldNotAccessApplicationLayer);

            Assert.IsTrue(combinedArchRule.HasNoViolations(Architecture));
        }
    }
}