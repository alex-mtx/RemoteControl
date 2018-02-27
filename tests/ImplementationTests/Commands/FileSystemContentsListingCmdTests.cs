using RC.Interfaces.Appenders;
using RC.Interfaces.Storages;
using Moq;
using NUnit.Framework;
using System;
using RC.Interfaces.Factories;
using RC.Implementation.Commands.Storages;
using RC.Domain.Storages;
using RC.Domain.Commands;

namespace ImplementationTests.Commands
{
    [TestFixture(Category = "Cmds", TestOf = typeof(FileSystemContentsListingCmd))]

    public class FileSystemContentsListingCmdTests
    {
        [Test]
        public void Should_Call_Storage_Contents()
        {
            var uri = new Uri("c:\\some.dll");
            var storageMock = new Mock<IStorage<IStorageObject>>();
            var appenderMock = new Mock<IResultAppender<CmdParametersSet>>();
            var storageFactoryMock = new Mock<IStorageFactory>();
            var cmdParam = new StorageCmdParamSet { Path = uri.ToString() };
            storageFactoryMock.Setup(x => x.Create(StorageType.FileSystem, uri)).Returns(storageMock.Object);
            var cmd = new FileSystemContentsListingCmd(storageFactoryMock.Object, appenderMock.Object, cmdParam);

            cmd.Run();

            storageMock.Verify(x => x.Contents(), Times.Once);
        }

    }
}
