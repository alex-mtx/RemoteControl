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
            var expectedUri = new Uri(AppContext.BaseDirectory);
            var actualSetup = new StorageSettings().GetSetup(expectedUri);
            var actualUri = actualSetup.Uri;
            Assert.AreEqual(expectedUri, actualUri);
        }
        [Test]
        [Ignore("The StorageSettings is yet not fully implemented.")]

        public void When_A_Uri_Is_Absolute_And_Relates_To_An_Non_Existing_StorageSetup_Then_Should_Throw_Exception()
        {
            var uriNotPresentOnSettings = new Uri(AppContext.BaseDirectory);

            var settings = new StorageSettings();

            Assert.Throws<ArgumentException>(() => settings.GetSetup(uriNotPresentOnSettings));
           
        }
        [Test]
        public void When_An_Instance_is_created_Then_Sets_Default_Strategy()
        {
            //Arrange
            var uriNotPresentOnSettings = new Uri(AppContext.BaseDirectory);
            var settings = new StorageSettings();

            //Act
            var setup = settings.GetSetup(uriNotPresentOnSettings);

            //Assert
            Assert.IsNotNull(setup);
            Assert.AreEqual(uriNotPresentOnSettings,setup.Uri);

        }
    }
}
