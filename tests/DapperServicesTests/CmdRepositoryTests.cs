using Dapper;
using Dapper.Contrib.Extensions;
using NUnit.Framework;
using RC.DapperServices;
using RC.DBMigrations;
using RC.Domain.Commands;
using RC.Implementation.Commands;
using RC.Implementation.Commands.Storages;
using RC.Infrastructure.Factories;
using RC.Interfaces.Factories;
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

            conn.Execute(@"INSERT INTO [CmdParametersSets] ([RequestId], [SentOn],[CmdType], [Path],[Status]) VALUES (@RequestId,@SentOn,@CmdType,@Path,@Status);", new
            {
                expectedCmd.RequestId,
                expectedCmd.SentOn,
                expectedCmd.CmdType,
                expectedCmd.Path,
                expectedCmd.Status
            });


            var actualCmds = new CmdRepository(_factory).PendingCommands();
            var actualCmd = actualCmds.Single();

            CollectionAssert.IsNotEmpty(actualCmds);
            Assert.True(actualCmds.Select(x => x.RequestId == expectedCmd.RequestId).Count() == 1);
            Assert.IsInstanceOf<StorageCmdParamSet>(actualCmd);
        }

        [Test]
        public void Update_Should_Change_Status_To_Executed()
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

          
            conn.Insert<CmdParametersSet>(newCmd);
            var persistedCmd = conn.Get<CmdParametersSet>(1);

            persistedCmd.Status = CmdStatus.Executed;

            new CmdRepository(_factory).Update(persistedCmd);

            persistedCmd = conn.Get<CmdParametersSet>(1);

            Assert.AreEqual(persistedCmd.Status, CmdStatus.Executed);

        }

        [Test]
        public void Update_Should_Change_Status_To_Executed_And_Populate_Result_Column()
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

            newCmd.Status = CmdStatus.Executed;
            newCmd.Result = "{\"name\":\"Joaquim\"}";

            conn.Insert<CmdParametersSet>(newCmd);
            var persistedCmd = conn.Get<CmdParametersSet>(1);



            new CmdRepository(_factory).Update(persistedCmd);

            persistedCmd = conn.Get<CmdParametersSet>(1);

            Assert.AreEqual(persistedCmd.Status, CmdStatus.Executed);

        }
    }
}
