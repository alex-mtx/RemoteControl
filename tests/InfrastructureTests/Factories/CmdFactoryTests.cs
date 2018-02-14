using NUnit.Framework;
using RC.Implementation.Commands;
using RC.Implementation.Commands.Storages;
using RC.Infrastructure.Factories;
using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace InfrastructureTests.Factories
{
    [TestFixture(Category = "Factories", TestOf = typeof(CmdFactory))]

    public class CmdFactoryTests
    {
        [Test]
        public void Create_Should_Create_FileSystemContentsListingCmd_Instance()
        {

            var expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString()
            };

            var actual = new CmdFactory().Create(CmdType.StorageContentsListing, expectedCmd);

            Assert.IsInstanceOf<FileSystemContentsListingCmd>(actual);


        }
    }
}
