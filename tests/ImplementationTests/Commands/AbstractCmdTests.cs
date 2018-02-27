using RC.Implementation.Commands;
using RC.Interfaces.Appenders;
using Moq;
using NUnit.Framework;
using RC.Domain.Commands;

namespace ImplementationTests.Commands
{
    [TestFixture(Category = "Cmds", TestOf = typeof(AbstractCmd<object>))]

    public class AbstractCmdTests
    {
        [Test]
        public void Should_Call_IResultAppender_Append_On_Every_Run_Call()
        {
            var appenderMock = new Mock<IResultAppender<CmdParametersSet>>();
            CmdParametersSet parameters = new CmdParametersSet();

            var cmd = new DummyCmd(appenderMock.Object, parameters);
            
            cmd.Run();
            appenderMock.Verify(x => x.Append(parameters), Times.Once);
        }
        
        private class DummyCmd : AbstractCmd<string>
        {
            public DummyCmd(IResultAppender<CmdParametersSet> resultAppender, CmdParametersSet args) : base(resultAppender, args)
            {
            }
                      
            protected override string RunCommand()
            {
                return "ran";
            }
        }
    }
}
