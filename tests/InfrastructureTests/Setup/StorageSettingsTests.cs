using NUnit.Framework;
using RC.Infrastructure.Setup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.Implementation.Storages;
using RC.JsonServices;
using RC.Interfaces.Infrastructure;
using Moq;
using RC.Interfaces.Storages;

namespace InfrastructureTests.Setup
{
    [TestFixture(TestOf =typeof(StorageSettings))]
    public class StorageSettingsTests
    {
        //[SetUp]
        //public void SetUp()
        //{
        //    var fullPath = Path.Combine(AppContext.BaseDirectory, "jsonsettings.json");
        //    if (!File.Exists(fullPath))
        //       File.Create(fullPath);

        //    List<BasicStorageSetup> defaultBasicStorageList = new List<BasicStorageSetup>()
        //    {
        //        new BasicStorageSetup(AppContext.BaseDirectory, "Uk", true),
        //        new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory)), "Es", true),
        //        new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory))), "Ftp", true)
        //    };

        //    var jsonBody = Json.Serialize(defaultBasicStorageList);

        //    File.WriteAllText(fullPath,jsonBody);
        //}

        
        [Test]
        public void When_A_Uri_Is_Absolute_And_Relates_To_An_Existing_StorageSetup_Then_Should_Return_An_Instance()
        {
            //Arrange
            var expectedUri = new Uri(AppContext.BaseDirectory);
            var strategyMock = new Mock<IStorageSettingsStrategy>();
            var storageSetupMock = new Mock<IStorageSetup>();
            storageSetupMock.Setup(x => x.Uri).Returns(expectedUri);
            strategyMock.Setup(x => x.GetSetup(It.IsAny<Uri>())).Returns(storageSetupMock.Object);
            var storageSettings = new StorageSettings(strategyMock.Object);
            
            //Act
            var actualSetup = storageSettings.GetSetup(expectedUri);
       
            //Arrange
            Assert.AreEqual(expectedUri, actualSetup.Uri);
        }
        [Test]
        //[Ignore("The StorageSettings is yet not fully implemented.")]

        public void When_A_Uri_Is_Absolute_And_Relates_To_An_Non_Existing_StorageSetup_Then_Should_Throw_Exception()
        {
            var uriNotPresentOnSettings = new Uri(AppContext.BaseDirectory);

            var strategyMock = new Mock<IStorageSettingsStrategy>();
            strategyMock.Setup(x => x.GetSetup(It.IsAny<Uri>())).Throws<ArgumentException>();


            var settings = new StorageSettings(strategyMock.Object);

            Assert.Throws<ArgumentException>(() => settings.GetSetup(uriNotPresentOnSettings));
           
        }
    }
}
