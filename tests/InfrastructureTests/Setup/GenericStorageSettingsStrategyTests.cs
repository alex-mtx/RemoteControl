using Moq;
using NUnit.Framework;
using RC.Implementation.Storages;
using RC.Infrastructure.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests.Setup
{
    [TestFixture(TestOf = typeof(GenericStorageSettingsStrategy))]
    public class GenericStorageSettingsStrategyTests
    {
        [Test]
        public void Should_Get_BasicStorageSetup_From_GetSetup()
        {
            //Arrange
            var strategyTest = new Mock<GenericStorageSettingsStrategy>();
            var uri = "C:\\";
            var basicStorageSetup = new BasicStorageSetup(uri, "nome", true);
            strategyTest.Setup(x => x.BuildSetups()).Returns(new List<BasicStorageSetup>()
            {
                basicStorageSetup
            });

            //Act
            var result = strategyTest.Object.GetSetup(new Uri(uri));

            //Assert
            Assert.AreSame(basicStorageSetup, result);
        }
    }
}
