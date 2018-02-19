using NUnit.Framework;
using RC.Implementation.Storages;
using System;
using System.IO;
using System.Linq;

namespace ImplementationTests.Storages
{
    [TestFixture]
    public class LocalFileSystemStorageTests
    {
        [Test(TestOf = typeof(LocalFileSystemStorage))]
        public void Lists_Its_Contents()
        {
            var storageUri = new Uri(AppDomain.CurrentDomain.BaseDirectory);
            var setup = new BasicStorageSetup(storageUri, "a name", true);
            var simpleStorage = new LocalFileSystemStorage(setup);
            var expected = Directory.EnumerateFileSystemEntries(storageUri.OriginalString, "*", SearchOption.AllDirectories);

            var actual = simpleStorage.Contents();

            CollectionAssert.AreEqual(expected, actual.Select(x => x.Uri.OriginalString));


        }
    }
}
