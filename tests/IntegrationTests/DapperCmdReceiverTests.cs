using Dapper;
using NUnit.Framework;
using RC.DapperServices;
using RC.DapperServices.Receivers;
using RC.DBMigrations;
using RC.Implementation.Commands.Storages;
using RC.Infrastructure.Factories;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using RC.SQLiteServices;
using System;
using System.Threading;

namespace IntegrationTests
{
    [TestFixture(Category = "IntegrationTests")]
    public class DapperCmdReceiverTests
    {
        private readonly string cs = "Data Source=|DataDirectory|demo2.db;Version=3";
        [Test]
        public void StartReceiving_When_A_New_Cmd_Is_Available_Then_Executes_Client_Delegate()
        {

            var migrator = new DebugMigrator(cs);
            migrator.Migrate();

            var factory = new SQLiteConnectionFactory(cs);
            var conn = factory.CreateDbConnection();

            var cmdFactory = new CmdFactory();
            var dbConnectionFactory = new SQLiteConnectionFactory(cs);
            var cmdRepository = new CmdRepository(dbConnectionFactory);
            var receiver = new DapperCmdReceiver(1, cmdFactory,cmdRepository);
            var executed = false;

            InsertCmd();

            Assert.DoesNotThrow(()=> receiver.StartReceiving((ICmd cmd) => { cmd.Run(); executed = true; }));

            Thread.Sleep(1500);
            Assert.True(executed);

        }

      
        private void InsertCmd()
        {
            var factory = new SQLiteConnectionFactory(cs);
            using (var conn = factory.CreateDbConnection())
            {
                var expectedCmd = new StorageCmdParamSet
                {
                    CmdType = CmdType.StorageContentsListing,
                    RequestId = Guid.NewGuid(),
                    SentOn = DateTime.Now,
                    Path = new Uri(AppDomain.CurrentDomain.BaseDirectory).AbsolutePath
                };

                conn.Execute(@"INSERT INTO [command_request] ([RequestId], [SentOn],[CmdType], [Path],[Finished]) VALUES (@RequestId,@SentOn,@CmdType,@Path,@Finished);", new
                {
                    expectedCmd.RequestId,
                    expectedCmd.SentOn,
                    expectedCmd.CmdType,
                    expectedCmd.Path,
                    expectedCmd.Finished
                });
            }
        }
    }
}
