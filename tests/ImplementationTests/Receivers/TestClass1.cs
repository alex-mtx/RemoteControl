using Moq;
using NUnit.Framework;
using RC.Implementation.Appenders;
using RC.Implementation.Commands;
using RC.Implementation.Receivers;
using RC.Implementation.Storages;
using RC.Infrastructure.Factories;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using RC.Interfaces.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationTests.Receivers
{
    [TestFixture]
    public class TestClass1
    {
        [Test]
        public void TestMethod()
        {
            var cmd = new StringCmdReceiver().Receive();
            Assert.IsInstanceOf<FileSystemContentsListingCmd>(cmd);
            Assert.DoesNotThrow(()=>cmd.Run());
        }

        private class StringCmdReceiver : ICommandReceiver<ICmd>
        {
            public ICmd Receive()
            {
                var json = $@"{{
                                'cmdType':'StorageContentsListing',
                                'requestId':'{Guid.NewGuid().ToString("N")}',
                                'uri':'{new Uri(AppDomain.CurrentDomain.BaseDirectory)}'
                                }}";
                var args = RC.JsonServices.Json.Deserialize<Dictionary<string, string>>(json);

                var cmdType = (CmdType)Enum.Parse(typeof(CmdType), args["cmdType"]);

                var cmd = new CmdFactory().Create(cmdType, args);

                return cmd;
            }
        }

        private class DummyCmd : ICmd
        {
            public void Run()
            {
            }
        }

        private class Factory
        {

        }


    }
}
