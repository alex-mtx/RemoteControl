using NUnit.Framework;
using RC.DBMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Factories;
using System.Reflection;
using Dapper.Contrib.Extensions;
using RC.SQLiteServices;
using RC.DapperServices;
using RC.Implementation.Commands;

namespace DapperServicesTests
{
    [TestFixture(TestOf =typeof(CmdRepository))]
    public class CmdRepositoryTests
    {

        [Test]
        public void PendingCommands_When_New_Command_Is_Available_List_It()
        {
            var migrator = new DebugMigrator("Data Source=|DataDirectory|demo.db;Version=3");
            migrator.Migrate(runner => runner.MigrateDown(0));
            migrator.Migrate(runner => runner.MigrateUp());

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
