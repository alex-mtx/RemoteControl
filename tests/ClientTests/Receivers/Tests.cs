using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RC.Client;
using RC.Client.Interfaces;
using RC.Client.Interfaces.Repositories;
using RC.Data;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientReceiverResultTests
{
    [TestFixture(Category = "ClientResultReceiver")]
    public class ClientTests
    {

        [Test]
        public void Should_Deserialize_Json_Result_To_Specified_CmdResult_Type()
        {
            string cmdResultJson = @"{
                                    ""Result"":""new result"",
                                    ""CmdParamsSet"":{
                                                    ""Path"":""C:\\dev\\git\\RemoteControl\\tests\\DapperServicesTests\\bin\\Debug\\"",
                                    ""Id"":1,
                                    ""RequestId"":""8925640c-ecf6-4f8c-b6c4-bc73b5354210"",
                                    ""SentOn"":""2018-03-01T17:26:37.147"",
                                    ""CmdType"":1,
                                    ""Status"":2}}";

            CmdResult<string, StorageCmdParamSet> cmdResult = JsonConvert.DeserializeObject<CmdResult<string, StorageCmdParamSet>>(cmdResultJson);
            Assert.AreEqual("new result", cmdResult.Result);
            Assert.AreEqual(Guid.Parse("8925640c-ecf6-4f8c-b6c4-bc73b5354210"), cmdResult.CmdParamsSet.RequestId);
        }


        [Test]
        public async Task Tests()
        {
            var expectedCmdParams = new CmdParametersSet {
                CmdType = CmdType.StorageContentsListing,
                Issuer = "myself",
                RequestId = Guid.NewGuid(),
                Status = CmdStatus.Executed,
                SentOn = DateTime.Now
            };

            var expectedResult = "test";
            var expectedCmdResult = new CmdResult<string, CmdParametersSet>(expectedResult, expectedCmdParams);

            var repoMock = new Mock<ICmdRepositoryAsync>();
            repoMock.Setup(x => x.RetrieveResultAsync<string, CmdParametersSet>(It.IsAny<CmdParametersSet>())).ReturnsAsync(() => expectedCmdResult);
            var remoteCmdCaller = new RemoteCommandCaller(repoMock.Object);

            var actualCmdResult = await remoteCmdCaller.CallAsync<string, CmdParametersSet>(new CmdParametersSet());

            repoMock.Verify(x => x.SendToBackendAsync(It.IsAny<CmdParametersSet>()),Times.Once);
            repoMock.Verify(x => x.RetrieveResultAsync<string,CmdParametersSet>(It.IsAny<CmdParametersSet>()), Times.Once);
            Assert.AreEqual(expectedResult, actualCmdResult.Result);
            Assert.AreEqual(expectedCmdResult, actualCmdResult);
        }

       

       
    }
}
