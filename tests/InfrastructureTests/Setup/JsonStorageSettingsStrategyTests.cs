using NUnit.Framework;
using RC.Implementation.Storages;
using RC.Infrastructure.Setup;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;
using System.IO;

namespace InfrastructureTests.Setup
{
    [TestFixture(TestOf =typeof(JsonStorageSettingsStrategy))]
    public class JsonStorageSettingsStrategyTests
    {
        private static IEnumerable<BasicStorageSetup> AddBasicStorageSetup()
        {
            yield return new BasicStorageSetup(AppContext.BaseDirectory, "Uk", true);
            yield return new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory)), "Es", true);
            yield return new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory))), "Ftp", true);
        }

        [Test]
        public void Should_Read_From_Relative_JsonFile()
        {
            //Arrange
            var jsonFileName = "jsonsettings.json";

            //Act & Assert
            Assert.DoesNotThrow(() => new JsonStorageSettingsStrategy(jsonFileName));
        }

        [Test]
        public void Should_Read_From_Absolute_JsonFile()
        {
            //Arrange
            var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "jsonsettings.json");

            //Act & Assert
            Assert.DoesNotThrow(() => new JsonStorageSettingsStrategy(jsonFilePath));
        }

        [Test,TestCaseSource("AddBasicStorageSetup")]
        public void Get_BasicStorage_Read_From_JsonFile(IStorageSetup setup)
        {
            //Arrange
            var expectedUri = setup.Uri;
            BasicStorageSetup expectedBasicStorageSetup = new BasicStorageSetup(expectedUri.AbsolutePath, setup.Name, setup.Active);
            var jsonFileName = "jsonsettings.json";
            var instance = new JsonStorageSettingsStrategy(jsonFileName);

            //Act
            var result = instance.GetSetup(expectedUri);

            //Assert
            Assert.AreEqual(expectedBasicStorageSetup.Uri, result.Uri);
            Assert.AreEqual(expectedBasicStorageSetup.Name, result.Name);
            Assert.AreEqual(expectedBasicStorageSetup.Active, result.Active);
        }
    }
}
