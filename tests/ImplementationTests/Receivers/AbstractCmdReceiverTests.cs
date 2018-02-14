using Moq;
using NUnit.Framework;
using RC.Implementation.Receivers;
using RC.Interfaces.Commands;
using RC.Interfaces.Receivers;

namespace ImplementationTests.Receivers
{
    [TestFixture(TestOf = typeof(AbstractCmdReceiver))]
    public class AbstractCmdReceiverTests
    {
        [Test]
        public void StartReceiving_When_A_Cmd_Is_Fetch_Should_Execute_Client_Delegate()
        {
            var cmdMock = new Mock<ICmd>();
            var receiver = new DummyCmdReceiver(cmdMock.Object);
            receiver.StartReceiving((ICmd cmd) => { cmd.Run(); });

            cmdMock.Verify(cmd => cmd.Run(), Times.Once);   
        }

        private class DummyCmdReceiver : AbstractCmdReceiver
        {
            private ICmd _cmd;

            public DummyCmdReceiver(ICmd cmd)
            {
                _cmd = cmd;
            }

            public override void StartReceiving(CmdReceivedEventHandler handler)
            {
                handler(_cmd);
            }

            public override void StopReceiving()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
