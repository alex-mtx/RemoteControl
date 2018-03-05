using Dapper;
using NUnit.Framework;
using RC.DapperServices;
using RC.DapperServices.Appenders;
using RC.DapperServices.Receivers;
using RC.DBMigrations;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Implementation.Appenders;
using RC.Implementation.Commands.Storages;
using RC.Implementation.Storages;
using RC.Infrastructure;
using RC.Infrastructure.Factories;
using RC.Infrastructure.Setup;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
        private IEnumerable<IOutput<CmdParametersSet>> RegisteredOutputs()
        {
            //TODO: get it from anywhere, e.g. app.config, database, just like log4net does
            return new List<IOutput<CmdParametersSet>>
            {
                new DebugConsoleOutput(),
                new DapperOutput(new CmdRepository(_factory))
            };
        }

        [Test]
        public void StartReceiving_When_A_New_Cmd_Is_Available_Then_Executes_Client_Delegate()
        {
            //Arrange
            var storageSettings = new StorageSettings(new JsonStorageSettingsStrategy("jsonsettings.json"));
            var storageFactory = new StorageFactory(storageSettings);

            var resultAppender = new GeneralResultAppender(RegisteredOutputs());
            var cmdFactory = new CmdFactory(resultAppender, storageFactory);
            var cmdRepository = new CmdRepository(_factory);
            var receiver = new DapperCmdReceiver(1, cmdFactory, cmdRepository);
            var executed = false;
            var exceptionThrown = false;
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { exceptionThrown = true; };
            TaskScheduler.UnobservedTaskException += (s, e) => { exceptionThrown = true; };

            InsertCmd(AppDomain.CurrentDomain.BaseDirectory);

            Assert.DoesNotThrow(() => receiver.StartReceiving((ICmd cmd) => { cmd.Run(); executed = true; }));
            Thread.Sleep(2000);
            Assert.IsFalse(exceptionThrown,"it is not expected to have exceptions!");
            Assert.True(executed);


        }

        [Test]
        public void StartReceiving_When_Cmd_Throws_Exception_Persist_The_Result_With_Exception()
        {
            //Arrange
            var storageFactory = new StorageFactory(new DummyStorageSettings());

            var resultAppender = new GeneralResultAppender(RegisteredOutputs());
            var cmdFactory = new CmdFactory(resultAppender, storageFactory);
            var cmdRepository = new CmdRepository(_factory);
            var receiver = new DapperCmdReceiver(1, cmdFactory, cmdRepository);

            InsertCmd("z:\\"); //this is a non existing path, so the listing command will throw an exception

            Assert.DoesNotThrow(() => receiver.StartReceiving((ICmd cmd) => { cmd.Run(); }));
            Thread.Sleep(2000);
            var persistedCmdResult = GetPersistedResult<Exception, StorageCmdParamSet>();
            Assert.IsAssignableFrom(typeof(Exception), persistedCmdResult.Result);
            Assert.AreEqual(CmdStatus.ResultedInError, persistedCmdResult.CmdParamsSet.Status);

        }

        private CmdResult<TResult,TCmdParams> GetPersistedResult<TResult,TCmdParams>() where TCmdParams : CmdParametersSet
        {
            var actualResultData = _factory.CreateDbConnection().ExecuteScalar<string>("SELECT CmdResultJson FROM [CmdParametersSets] WHERE Id= 1");

            var persistedCmdResult = RC.JsonServices.Json.Deserialize<CmdResult<TResult, TCmdParams>>(actualResultData);
            return persistedCmdResult;
        }

        private class DummyStorageSettings : IStorageSettings
        {
          
            public IStorageSetup GetSetup(Uri uri)
            {
                return new BasicStorageSetup(uri.AbsolutePath, "fake repo", true);
            }
        }

        [Test]
        public void StartReceiving_When_A_New_Cmd_ToListFiles()
        {
            var storageSettings = new StorageSettings(new JsonStorageSettingsStrategy("jsonsettings.json"));
            var storageFactory = new StorageFactory(storageSettings);
            var cmdFactory = new CmdFactory(ResultAppenderManager.Instance.ResultAppender, storageFactory);
            var cmdRepository = new CmdRepository(_factory);
            var receiver = new DapperCmdReceiver(1, cmdFactory, cmdRepository);
            var executed = false;

            InsertCmd(AppDomain.CurrentDomain.BaseDirectory);

            Assert.DoesNotThrow(() => receiver.StartReceiving((ICmd cmd) => { cmd.Run(); executed = true; }));

            Thread.Sleep(1500);
            Assert.True(executed);
        }

        private void InsertCmd(string path)
        {
            using (var conn = _factory.CreateDbConnection())
            {
                var expectedCmd = new StorageCmdParamSet
                {
                    CmdType = CmdType.StorageContentsListing,
                    RequestId = Guid.NewGuid(),
                    SentOn = DateTime.Now,
                    Path = new Uri(path).AbsolutePath,
                    Status = CmdStatus.AwaitingForExecution,
                    Issuer = "me"

                };

                conn.Execute(@"INSERT INTO [CmdParametersSets] ([RequestId], [SentOn],[CmdType], [Path],[Status],[Issuer]) VALUES (@RequestId,@SentOn,@CmdType,@Path,@Status,@Issuer);", new
                {
                    expectedCmd.RequestId,
                    expectedCmd.SentOn,
                    expectedCmd.CmdType,
                    expectedCmd.Path,
                    expectedCmd.Status,
                    expectedCmd.Issuer
                });
            }
        }
    }
}
