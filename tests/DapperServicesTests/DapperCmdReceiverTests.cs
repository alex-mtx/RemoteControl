using Moq;
using NUnit.Framework;
using RC.DapperServices.Receivers;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using RC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DapperServicesTests
{
    [TestFixture(Category = "DapperServices", TestOf = typeof(DapperCmdReceiver))]
    public class DapperCmdReceiverTests
    {
        [Test]
        public void StartReceiving_When_A_New_Cmd_Is_Available_Then_Executes_Client_Delegate()
        {
            var interval = 1;
            var cmdMock = new Mock<ICmd>();
            cmdMock.Setup(x => x.Run());
            var repoMock = new Mock<ICmdRepository<CmdParametersSet, CmdParametersSet>>();
            repoMock.Setup(x => x.PendingCommands()).Returns(() => GenerateCmds());
            var factoryMock = new Mock<ICmdFactory<CmdParametersSet>>();
            factoryMock.Setup(x => x.Create(It.IsAny<CmdType>(), It.IsAny<CmdParametersSet>())).Returns(cmdMock.Object);

            var receiver = new DapperCmdReceiver(interval, factoryMock.Object, repoMock.Object);

            receiver.StartReceiving((ICmd cmd) => { cmd.Run(); });
            Thread.Sleep(500);
            cmdMock.Verify(cmd => cmd.Run(), Times.Once);
        }

     

        private List<CmdParametersSet> GenerateCmds()
        {
            var cmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                Status = CmdStatus.AwaitingForExecution,
                Path = new Uri(AppDomain.CurrentDomain.BaseDirectory).OriginalString,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now
            };
            return new List<CmdParametersSet> { cmd };
        }

    }
}
