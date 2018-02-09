using NUnit.Framework;
using RC.Implementation.Commands;
using RC.Infrastructure.Factories;
using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace InfrastructureTests.Factories
{
    [TestFixture(TestOf = typeof(CmdFactory))]
    public class CmdFactoryTests
    {
        [Test]
        public void Create_Should_Create_FileSystemContentsListingCmd_Instance()
        {
           
            var args = new Dictionary<string, string>
            {
                {"cmdType","StorageContentsListing" },
                {"requestId",Guid.NewGuid().ToString("N") },
                {"uri",new Uri(AppDomain.CurrentDomain.BaseDirectory).ToString() }
            };

            var actual = CmdFactory.Create(CmdType.StorageContentsListing, args);

            Assert.IsInstanceOf<FileSystemContentsListingCmd>(actual);


        }
    }
}
