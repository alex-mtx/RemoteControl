using Dapper;
using Dapper.Contrib.Extensions;
using NUnit.Framework;
using RC.Client.DapperServices;
using RC.Data;
using RC.DBMigrations;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Domain.Storages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace ClientDapperServicesTests
{
    [TestFixture(Category = "DapperServices", TestOf = typeof(DapperCmdRepository))]
    public class DapperCmdRepositoryTests
    {
        private static ConnectionStringSettings connectionSettings = ConfigurationManager.ConnectionStrings["default"];
        private DBConnectionFactory _factory;

        [SetUp]
        public void SetUp()
        {
            var migrator = new DebugMigrator(connectionSettings.ConnectionString);
            migrator.Migrate("SQLServer");
            _factory = new DBConnectionFactory(connectionSettings);
            SqlMapperExtensions.TableNameMapper += (type) => { return type.TableName(); };

        }
        [Test]
        public async Task Should_Record_Cmd()
        {
            var repo = new DapperCmdRepository(_factory);
            var expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                Issuer = "me",
                Path = "c:\\",
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Status = CmdStatus.AwaitingForExecution
            };

            await repo.SendToBackendAsync(expectedCmd);

            var actualCmd = _factory.CreateDbConnection().Get<StorageCmdParamSet>(1);

            Assert.AreEqual(expectedCmd.RequestId, actualCmd.RequestId);
            Assert.AreEqual(expectedCmd.Path, actualCmd.Path);
        }

        [Test]
        public async Task Should_Not_Retrieve_Cmd_Awaiting_To_Be_Executed()
        {
            var repo = new DapperCmdRepository(_factory);
            StorageCmdParamSet expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                Issuer = "me",
                Path = "c:\\",
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Status = CmdStatus.AwaitingForExecution
            };

            _factory.CreateDbConnection().Insert(expectedCmd);

            var actualCmdResult = await repo.RetrieveExecutedCmdAsync(expectedCmd);

            Assert.AreEqual(default(StorageCmdParamSet),actualCmdResult);
        }

        [Test]
        [TestCase(CmdStatus.Executed)]
        [TestCase(CmdStatus.Malformed)]
        [TestCase(CmdStatus.ResultedInError)]
        public async Task Should_Retrieve_Cmd_With_Specified_Status_Except_Awaiting_To_Be_Executed(CmdStatus status)
        {
            var repo = new DapperCmdRepository(_factory);
            StorageCmdParamSet expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                Issuer = "me",
                Path = "c:\\",
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Status = status
            };

            _factory.CreateDbConnection().Insert(expectedCmd);

            var actualCmdResult = await repo.RetrieveExecutedCmdAsync(expectedCmd);

            Assert.AreEqual(status, actualCmdResult.Status);
        }

        [Test]
        public async Task Should_Retrieve_Executed_Cmd_JsonResult_As_A_String()
        {
            var repo = new DapperCmdRepository(_factory);
            StorageCmdParamSet expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                Issuer = "me",
                Path = "c:\\",
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Status = CmdStatus.Executed,
            };

            var cmdResult = new CmdResult<string, CmdParametersSet>
            {
                CmdParamsSet = expectedCmd,
                Result = "new result"
            };

            _factory.CreateDbConnection().Insert(expectedCmd);

            var expectedJsonResultData = RC.JsonServices.Json.Serialize(cmdResult);

            string sql = $"UPDATE {typeof(StorageCmdParamSet).TableName()} SET CmdResultJson = @CmdResult WHERE ID = @Id";
            _factory.CreateDbConnection().Execute(sql, new { CmdResult = expectedJsonResultData,Id=1 });
            
            var actualCmdJsonResult = await repo.RetrieveExecutedCmdResultJsonAsync(expectedCmd);

            Assert.AreEqual(expectedJsonResultData,actualCmdJsonResult);
        }
    }
}
