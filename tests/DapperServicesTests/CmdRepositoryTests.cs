using Dapper;
using NUnit.Framework;
using RC.DapperServices;
using RC.DBMigrations;
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

        [Test]
        public void PendingCommands_When_New_Command_Is_Available_List_It()
        {
            var migrator = new DebugMigrator("Data Source=|DataDirectory|demo.db;Version=3");
            migrator.Migrate();

            var factory = new SQLiteConnectionFactory("Data Source=|DataDirectory|demo.db;Version=3");
            var conn = factory.CreateDbConnection();
            
            var expectedCmd = new StorageCmdParamSet
            {
                CmdType = CmdType.StorageContentsListing,
                RequestId = Guid.NewGuid(),
                SentOn = DateTime.Now,
                Path = AppDomain.CurrentDomain.BaseDirectory.ToString()
            };

            conn.Execute(@"INSERT INTO [command_request] ([RequestId], [SentOn],[CmdType], [Path],[Finished]) VALUES (@RequestId,@SentOn,@CmdType,@Path,@Finished);", new
            {
                expectedCmd.RequestId,
                expectedCmd.SentOn,
                expectedCmd.CmdType,
                expectedCmd.Path,
                expectedCmd.Finished
            });


            var actualCmds = new CmdRepository(factory).PendingCommands();
            var actualCmd = actualCmds.Single();

            CollectionAssert.IsNotEmpty(actualCmds);
            Assert.True(actualCmds.Select(x => x.RequestId == expectedCmd.RequestId).Count() == 1);
            Assert.IsInstanceOf<StorageCmdParamSet>(actualCmd);
        }
    }
}
