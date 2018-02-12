using NUnit.Framework;
using RC.DBMigrations;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Factories;
using System.Reflection;
using Dapper.Contrib.Extensions;

namespace DapperServicesTests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            var migrator = new DebugMigrator("Data Source=|DataDirectory|demo.db;Version=3");
            migrator.Migrate(runner => runner.MigrateDown(0));
            migrator.Migrate(runner => runner.MigrateUp());

            var conn = new SQLiteConnection("Data Source=|DataDirectory|demo.db;Version=3");
            
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
            var actualCmds = conn.Query<StorageCmdParamSet>("SELECT * from command_request WHERE [Finished] = 0");

            CollectionAssert.IsNotEmpty(actualCmds);
            Assert.True(actualCmds.Select(x => x.RequestId == expectedCmd.RequestId).Count() == 1);

        }
    }
}
