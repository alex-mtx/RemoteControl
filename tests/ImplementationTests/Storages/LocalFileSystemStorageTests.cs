using RC.Implementation.Storages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationTests.Storages
{
    [TestFixture]
    public class LocalFileSystemStorageTests
    {
        [Test]
        public void Lists_Its_Contents()
        {
            var storageUri = new Uri(AppDomain.CurrentDomain.BaseDirectory);
            var setup = new BasicStorageSetup(storageUri);
            var simpleStorage = new LocalFileSystemStorage(setup);
            var expected = Directory.EnumerateFileSystemEntries(storageUri.OriginalString, "*", SearchOption.AllDirectories);

            var actual = simpleStorage.Contents();

            CollectionAssert.AreEqual(expected, actual.Select(x => x.Uri.OriginalString));


        }
    }
}
