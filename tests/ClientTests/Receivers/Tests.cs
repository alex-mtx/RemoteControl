using Newtonsoft.Json;
using NUnit.Framework;
using RC.Client.Interfaces;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using System;
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
        public async void Tests()
        {
            var remoteCmdCaller = new RemoteCommandCaller();
            var result = await remoteCmdCaller.CallAsync<string, CmdParametersSet>(new CmdParametersSet());

        }

        private class RemoteCommandCaller : IRemoteCmdCaller
        {
            public RemoteCommandCaller()
            {
            }

            public  async Task<CmdResult<TResult, TCmdParamsSet>> CallAsync<TResult, TCmdParamsSet>(TCmdParamsSet cmdParams) where TCmdParamsSet : CmdParametersSet
            {
                return  await Task.Run(()=>new CmdResult<TResult, TCmdParamsSet>(default(TResult), cmdParams));
            }
        }
    }
}
