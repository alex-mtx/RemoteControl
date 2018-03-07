using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RC.Client.Commands;
using RC.Client.Interfaces;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Domain.Storages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientReceiverResultTests
{
    [TestFixture(Category = "ClientCommands")]
    public class AbstractCmdDispatcherTests
    {

        [Test]
        public async Task Should_Issue_StorageListingCmd_And_Get_Executed_Result()
        {
            var uri = new Uri("C:\\temp");
            var fakeSessionId = "currentUserName";
            var expectedCmdResult = new CmdResult<List<SimpleStorageObject>, StorageCmdParamSet>
            {
                Result = new List<SimpleStorageObject>() { new SimpleStorageObject(uri) },
                CmdParamsSet = new StorageCmdParamSet { Issuer = fakeSessionId, Status = CmdStatus.Executed }
            };
            var remoteCmdCallerMock = new Mock<IRemoteCmdCaller>();
            remoteCmdCallerMock.Setup(x => x.CallAsync<List<SimpleStorageObject>, StorageCmdParamSet>(It.IsAny<StorageCmdParamSet>())).ReturnsAsync(() => expectedCmdResult);
            var sender = new DummyDispatcher(remoteCmdCallerMock.Object);

            var actualCmdResult = await sender.SubmitAsync(fakeSessionId);

            Assert.AreEqual(fakeSessionId, actualCmdResult.CmdParamsSet.Issuer);
            Assert.AreEqual(CmdStatus.Executed, actualCmdResult.CmdParamsSet.Status);
            CollectionAssert.AreEqual(expectedCmdResult.Result, actualCmdResult.Result);

        }


        [Test]
        public void Should_Throw_ArgumentException_When_PerUserSessionId_Is_Invalid()
        {
            var sender = new DummyDispatcher(null);

            CmdResult<List<SimpleStorageObject>, StorageCmdParamSet> result = null;
            Assert.ThrowsAsync<ArgumentException>(async () =>
                result = await sender.SubmitAsync("")
            );
        }


        private class DummyDispatcher : AbstractCmdDispatcher<List<SimpleStorageObject>, StorageCmdParamSet>
        {

            public DummyDispatcher(IRemoteCmdCaller remoteCmdCaller) : base(remoteCmdCaller)
            {
            }

            protected override StorageCmdParamSet ParamsCompletion()
            {
                return new StorageCmdParamSet();
            }
        }
    }
}
