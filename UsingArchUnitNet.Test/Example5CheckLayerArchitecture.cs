using NUnit.Framework;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

namespace UsingArchUnitNet.Test
{
    public class Example5CheckLayerArchitecture
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(Class1).Assembly)
            .Build();

        //UsingArchUnitNet es el nombre de la librería a probar, luego todos los namespaces deberán empezar por UsingArchUnitNet
        private readonly IObjectProvider<IType> Layer = ArchRuleDefinition.Types().That().ResideInNamespace("UsingArchUnitNet");
        private readonly IObjectProvider<Interface> Interfaces = ArchRuleDefinition.Interfaces();
        private readonly IObjectProvider<Class> Classes = ArchRuleDefinition.Classes();

        [Test]
        public void All_Classes_Have_Correct_Namespace()
        {
            IArchRule classesHaveCorrectNamespace =
                ArchRuleDefinition.Classes().That().Are(Classes).Should().Be(Layer);
            IArchRule interfacesHaveCorrectNamespace =
                ArchRuleDefinition.Interfaces().That().Are(Interfaces).Should().Be(Layer);

            IArchRule combinedArchRule =
                classesHaveCorrectNamespace.And(interfacesHaveCorrectNamespace);

            Assert.IsTrue(combinedArchRule.HasNoViolations(Architecture));
        }
    }
}