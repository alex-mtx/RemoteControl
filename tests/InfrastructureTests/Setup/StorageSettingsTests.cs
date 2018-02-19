using NUnit.Framework;
using RC.Infrastructure.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests.Setup
{
    [TestFixture(TestOf =typeof(StorageSettings))]
    public class StorageSettingsTests
    {
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
    }
}
