using Moq;
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
    public class FileSystemContentsListingDispatcherTests
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
            var sender = new FileSystemContentsListingDispatcher(uri, remoteCmdCallerMock.Object);

            var actualCmdResult = await sender.SubmitAsync(fakeSessionId);

            Assert.AreEqual(fakeSessionId, actualCmdResult.CmdParamsSet.Issuer);
            Assert.AreEqual(CmdStatus.Executed, actualCmdResult.CmdParamsSet.Status);
            CollectionAssert.AreEqual(expectedCmdResult.Result, actualCmdResult.Result);

        }


        [Test]
        public void Should_Throw_ArgumentException_When_Uri_Is_Not_Absolute()
        {
            var uri = new Uri("\\temp",UriKind.Relative);
            Assert.Throws<ArgumentException>(()=> new FileSystemContentsListingDispatcher(uri, null));
            
        }

       
        

      

      

      


    }
}
