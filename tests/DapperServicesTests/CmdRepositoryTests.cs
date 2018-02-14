using Dapper;
using NUnit.Framework;
using RC.DapperServices;
using RC.DBMigrations;
using RC.Implementation.Commands;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Factories;
using RC.SQLiteServices;
using System;
using System.Linq;

namespace DapperServicesTests
{
    [TestFixture(Category = "DapperServices", TestOf =typeof(CmdRepository))]
    public class CmdRepositoryTests
    {

        //private static string Cs = "Data Source=|DataDirectory|dem1.db;Version=3";
        private static string Cs = "FullUri=file::memory:?cache=shared;";

        [Test]
        public void PendingCommands_When_New_Command_Is_Available_List_It()
        {
            var migrator = new DebugMigrator(Cs);
            migrator.Migrate();

            var factory = new SQLiteConnectionFactory(Cs);
            var conn = factory.CreateDbConnection();
            
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


            var actualCmds = new CmdRepository(factory).PendingCommands();
            var actualCmd = actualCmds.Single();

            CollectionAssert.IsNotEmpty(actualCmds);
            Assert.True(actualCmds.Select(x => x.RequestId == expectedCmd.RequestId).Count() == 1);
            Assert.IsInstanceOf<StorageCmdParamSet>(actualCmd);
        }

        [Test]
        public void ChangeStatusToExecuted_Should_Change_Status_To_Executed()
        {
            var migrator = new DebugMigrator(Cs);
            migrator.Migrate();

            var factory = new SQLiteConnectionFactory(Cs);
            var conn = factory.CreateDbConnection();

            var expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString(),
                Status = CmdStatus.AwaitingForExecution

            };

            var id = conn.Execute(@"INSERT INTO [CmdParametersSets] ([RequestId], [SentOn],[CmdType], [Path],[Status]) VALUES (@RequestId,@SentOn,@CmdType,@Path,@Status);", new
            {
                expectedCmd.RequestId,
                expectedCmd.SentOn,
                expectedCmd.CmdType,
                expectedCmd.Path,
                expectedCmd.Status
            });


            expectedCmd.Id = id;
            expectedCmd.Status = CmdStatus.Executed;

            new CmdRepository(factory).Update(expectedCmd);
            
            

        }
    }
}
