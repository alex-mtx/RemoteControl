using Dapper;
using Dapper.Contrib.Extensions;
using NUnit.Framework;
using RC.DapperServices;
using RC.Data;
using RC.DBMigrations;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using System;
using System.Configuration;
using System.Linq;

namespace DapperServicesTests
{
    [TestFixture(Category = "DapperServices", TestOf =typeof(CmdRepository))]
    public class CmdRepositoryTests
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
        public void PendingCommands_When_New_Command_Is_Available_List_It()
        {
            
            var conn = _factory.CreateDbConnection();

            var expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString(),
                Status = CmdStatus.AwaitingForExecution
            };

            conn.Insert(expectedCmd);

            var actualCmds = new CmdRepository(_factory).PendingCommands();
            var actualCmd = actualCmds.Single();

            CollectionAssert.IsNotEmpty(actualCmds);
            Assert.True(actualCmds.Select(x => x.RequestId == expectedCmd.RequestId).Count() == 1);
            Assert.IsInstanceOf<StorageCmdParamSet>(actualCmd);
        }

        [Test]
        public void Update_Should_Change_CmdParametersSet_Status_To_Executed()
        {
           
            var conn = _factory.CreateDbConnection();

            var newCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString(),
                Status = CmdStatus.AwaitingForExecution

            };

          
            conn.Insert(newCmd);
            var persistedCmd = conn.Get<CmdParametersSet>(1);

            persistedCmd.Status = CmdStatus.Executed;

            new CmdRepository(_factory).Update(persistedCmd);

            persistedCmd = conn.Get<CmdParametersSet>(1);

            Assert.AreEqual(persistedCmd.Status, CmdStatus.Executed);

        }

        [Test]
        public void Update_Should_Change_CmdResult_Status_To_Executed_And_Populate_Result_Column()
        {

            var conn = _factory.CreateDbConnection();

            var newCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString(),
                Status = CmdStatus.AwaitingForExecution

            };

            conn.Insert(newCmd);


            var persistedCmd = conn.Get<StorageCmdParamSet>(1);

            persistedCmd.Status = CmdStatus.Executed;
            var cmdResult = new CmdResult<string, CmdParametersSet>
            {
                CmdParamsSet = persistedCmd,
                Result = "new result"
            };
            var expectedJsonResultData = RC.JsonServices.Json.Serialize(cmdResult);


            //Act
            new CmdRepository(_factory).Update(cmdResult);

            //1st Get Updated Copy of parameter
            var actualPersistedCmd = conn.Get<StorageCmdParamSet>(1); 

            //2nd now only the Result, as it is not part of any Type
            var actualResultData = conn.ExecuteScalar<string>($"SELECT CmdResultJson FROM {typeof(StorageCmdParamSet).TableName()} WHERE Id=@Id", new { persistedCmd.Id });

            Assert.AreEqual(CmdStatus.Executed,actualPersistedCmd.Status );
            Assert.AreEqual(expectedJsonResultData, actualResultData);

        }


        [Test]
        public void Update_Should_Change_CmdResult_Status_To_ResultedInError_And_Populate_Result_With_Exception()
        {

            var conn = _factory.CreateDbConnection();
            var exception = new ArgumentException("unknown type", "paramName");

            var newCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString(),
                Status = CmdStatus.AwaitingForExecution

            };

            conn.Insert(newCmd);

            var persistedCmd = conn.Get<StorageCmdParamSet>(1);

            persistedCmd.Status = CmdStatus.ResultedInError;
            var cmdResult = new CmdResult<ArgumentException, CmdParametersSet>
            {
                CmdParamsSet = persistedCmd,
                Result = exception
            };
            var expectedJsonResultData = RC.JsonServices.Json.Serialize(cmdResult);


            //Act
            new CmdRepository(_factory).Update(cmdResult);

            //1st Get Updated Copy of parameter
            var actualPersistedCmd = conn.Get<StorageCmdParamSet>(1);

            //2nd now only the Result (json), as it is not part of any Type
            var actualResultData = conn.ExecuteScalar<string>($"SELECT CmdResultJson FROM {typeof(StorageCmdParamSet).TableName()} WHERE Id=@Id", new { persistedCmd.Id }); 

            CmdResult<ArgumentException, StorageCmdParamSet> persistedCmdResult = RC.JsonServices.Json.Deserialize<CmdResult<ArgumentException, StorageCmdParamSet>>(actualResultData);


            Assert.AreEqual(CmdStatus.ResultedInError, actualPersistedCmd.Status);
            Assert.AreEqual(expectedJsonResultData, actualResultData);
            Assert.IsAssignableFrom(exception.GetType(), actual: persistedCmdResult.Result);

        }
    }
}
