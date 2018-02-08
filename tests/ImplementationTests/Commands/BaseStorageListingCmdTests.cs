using RC.Implementation.Commands;
using RC.Implementation.Storages;
using RC.Interfaces.Appenders;
using RC.Interfaces.Storages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ImplementationTests.Commands
{
    [TestFixture]
    public class BaseStorageListingCmdTests
    {
        [Test]
        public void Should_Call_Storage_Contents()
        {
            var objects = new List<IStorageObject> { new SimpleStorageObject(new Uri("c:\\some.dll")) };
            var storageMock = new Mock<IStorage<IStorageObject>>();
            storageMock.Setup(x => x.Contents())
                    .Returns(() =>
                    {
                        return objects;
                    });
            var appenderMock = new Mock<IResultAppender>();
            appenderMock.Setup(x => x.Append(objects));

            var cmd = new FileSystemContentsListingCmd(storageMock.Object, appenderMock.Object);
            cmd.Run();

            storageMock.Verify(x => x.Contents(), Times.Once);
        }

    }
}
