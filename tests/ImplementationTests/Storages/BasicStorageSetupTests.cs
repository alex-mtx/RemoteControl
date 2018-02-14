using RC.Implementation.Storages;
using NUnit.Framework;
using System;

namespace ImplementationTests.Storages
{
    [TestFixture(Category = "Storages", TestOf = typeof(BasicStorageSetup))]

    public class BasicStorageSetupTests
    {
        [Test]
        public void When_Uri_Is_Absolute_It_Is_Accepted()
        {
            var absoluteUri = new Uri("C:\\");
            Assert.DoesNotThrow(() => new BasicStorageSetup(absoluteUri));
        }

        [Test]
        public void When_Uri_Is_Relative_It_Is_Not_Accepted()
        {
            var relative = new Uri("\\var", UriKind.Relative);
            Assert.Throws<ArgumentException>(() => new BasicStorageSetup(relative));
        }
    }
}
