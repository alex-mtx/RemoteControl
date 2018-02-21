using Dapper;
using NUnit.Framework;
using RC.DapperServices;
using RC.DapperServices.Receivers;
using RC.DBMigrations;
using RC.Domain.Commands;
using RC.Implementation.Commands.Storages;
using RC.Infrastructure;
using RC.Infrastructure.Factories;
using RC.Infrastructure.Setup;
using RC.Interfaces.Commands;
using System;
using System.Configuration;
using System.Threading;

namespace IntegrationTests
{
    [TestFixture(Category = "IntegrationTests")]
    public class DapperCmdReceiverTests
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
        public void StartReceiving_When_A_New_Cmd_Is_Available_Then_Executes_Client_Delegate()
        {
            //Arrange
            var storageSettings = new StorageSettings(new JsonStorageSettingsStrategy("file"));
            var storageFactory = new StorageFactory(storageSettings);
            var cmdFactory = new CmdFactory(ResultAppenderManager.Instance.ResultAppender, storageFactory);
            var cmdRepository = new CmdRepository(_factory);
            var receiver = new DapperCmdReceiver(1, cmdFactory,cmdRepository);
            var executed = false;

            InsertCmd();

            Assert.DoesNotThrow(()=> receiver.StartReceiving((ICmd cmd) => { cmd.Run(); executed = true; }));

            Thread.Sleep(1500);
            Assert.True(executed);

        }

      
        private void InsertCmd()
        {
            using (var conn = _factory.CreateDbConnection())
            {
                var expectedCmd = new StorageCmdParamSet
                {
                    CmdType = CmdType.StorageContentsListing,
                    RequestId = Guid.NewGuid(),
                    SentOn = DateTime.Now,
                    Path = new Uri(AppDomain.CurrentDomain.BaseDirectory).AbsolutePath,
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
            }
        }
    }
}
