using Newtonsoft.Json;
using NUnit.Framework;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Implementation.Commands.Storages;
using System;
using System.Collections.Generic;

namespace ClientReceiverResultTests
{
    [TestFixture(Category = "ClientReceiverResult")]
    public class ClientReceiverResultTest
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
        public void Should_Issue_StorageListingCmd()
        {
            //var cmdManager = new CmdManager();
            //cmdManager.Issue(CmdType.StorageContentsListing)
        }

    
    }
}
