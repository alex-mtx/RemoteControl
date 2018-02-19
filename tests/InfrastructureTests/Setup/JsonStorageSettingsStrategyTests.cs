using NUnit.Framework;
using RC.Implementation.Storages;
using RC.Infrastructure.Setup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests.Setup
{
    [TestFixture(TestOf =typeof(JsonStorageSettingsStrategy))]
    public class JsonStorageSettingsStrategyTests
    {
        [TestCase("C:\\dev\\git\\RemoteControl\\tests\\DapperServicesTests\\bin\\Debug", "uk",true)]
        public void Is_Singleton(string path,string name,bool isActive)
        {
            //Arrange
            var expectedUri = new Uri(path);
            BasicStorageSetup expectedBasicStorageSetup = new BasicStorageSetup(expectedUri, name, isActive);
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
