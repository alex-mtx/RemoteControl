using RC.Implementation.Commands;
using RC.Interfaces.Appenders;
using Moq;
using NUnit.Framework;

namespace ImplementationTests.Commands
{
    [TestFixture]
    public class AbstractCmdTests
    {
        [Test]
        public void Should_Call_IResultAppender_Append_On_Every_Run_Call()
        {
            var appenderMock = new Mock<IResultAppender>();

            var cmd = new DummyCmd(appenderMock.Object);
            cmd.Run();
            appenderMock.Verify(x => x.Append<object>(null), Times.Once);
        }


        class DummyCmd : AbstractCmd<object>
        {
            public DummyCmd(IResultAppender resultAppender) : base(resultAppender)
            {
            }

            protected override void AppendResult(object result)
            {
                base.AppendResult(result);
            }

            protected override object RunCommand()
            {
                return null;
            }
        }
    }
}
