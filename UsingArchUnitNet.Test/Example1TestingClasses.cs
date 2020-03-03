using NUnit.Framework;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace UsingArchUnitNet.Test
{
    public class Example1TestingClasses
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(Class1).Assembly)
            .Build();

        private Interface _baseInterface;

        [SetUp]
        public void Setup()
        {
            _baseInterface = Architecture.GetInterfaceOfType(typeof(Contracts.IBaseInterface));
        }

        [Test]
        public void All_Classes_Implements_BaseInterface()
        {
            Assert.IsTrue(Architecture.Classes.All(archClass => archClass.ImplementsInterface(_baseInterface)));
        }

        [Test]
        public void All_Public_Properties_Start_UpperCase()
        {
            Assert.IsTrue(Architecture.Classes.All(archClass => GetPropertiesByType(archClass).All(prop => char.IsUpper(prop.Name.First()))));
        }

        private IEnumerable<PropertyMember> GetPropertiesByType(Class archClass)
        {
            return archClass.GetPropertyMembers().Where(prop => prop.Visibility == Visibility.Public);
        }
    }
}