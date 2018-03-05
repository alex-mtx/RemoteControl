using NUnit.Framework;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Implementation.Commands.Storages;
using RC.Infrastructure.Factories;
using RC.Interfaces.Appenders;
using RC.Interfaces.Factories;
using System;

namespace InfrastructureTests.Factories
{
    [TestFixture(Category = "Factories", TestOf = typeof(CmdFactory))]

    public class CmdFactoryTests
    {
        [Test]
        public void Create_Should_Create_FileSystemContentsListingCmd_Instance()
        {
            var resultAppenderMock = new Moq.Mock<IResultAppender<CmdParametersSet>>();
            var storageFactoryMock = new Moq.Mock<IStorageFactory>();
            var expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString()
            };

            var actual = new CmdFactory(resultAppenderMock.Object, storageFactoryMock.Object).Create(CmdType.StorageContentsListing, expectedCmd);

            Assert.IsInstanceOf<FileSystemContentsListingCmd>(actual);


        }
    }
}
