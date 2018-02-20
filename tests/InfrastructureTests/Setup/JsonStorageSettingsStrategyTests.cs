using NUnit.Framework;
using RC.Implementation.Storages;
using RC.Infrastructure.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using RC.Interfaces.Storages;
using RC.JsonServices;

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

        [Test,TestCaseSource("AddBasicStorageSetup")]
        public void Get_BasicStorage_Read_From_JsonFile(IStorageSetup setup)
        {
            //Arrange
            var expectedUri = setup.Uri;
            BasicStorageSetup expectedBasicStorageSetup = new BasicStorageSetup(expectedUri.AbsolutePath, setup.Name, setup.Active);
            var instance = JsonStorageSettingsStrategy.Instance;

            //Act
            var result = instance.GetSetup(expectedUri);

            //Assert
            Assert.AreEqual(expectedBasicStorageSetup.Uri, result.Uri);
            Assert.AreEqual(expectedBasicStorageSetup.Name, result.Name);
            Assert.AreEqual(expectedBasicStorageSetup.Active, result.Active);
        }
    }
}
