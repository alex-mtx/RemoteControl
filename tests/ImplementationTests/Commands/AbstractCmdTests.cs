using RC.Implementation.Commands;
using RC.Interfaces.Appenders;
using Moq;
using NUnit.Framework;
using RC.Domain.Commands;
using System;

namespace ImplementationTests.Commands
{
    [TestFixture(Category = "Cmds", TestOf = typeof(AbstractCmd<CmdParametersSet,string>))]

    public class AbstractCmdTests
    {
        [Test]
        public void Should_Call_IResultAppender_Append_On_Every_Run_Call()
        {
            var appenderMock = new Mock<IResultAppender<CmdParametersSet>>();
            CmdParametersSet parameters = new CmdParametersSet();

            var cmd = new DummyCmd(appenderMock.Object, parameters);
            
            cmd.Run();
            appenderMock.Verify(x => x.Append(parameters,"ran"), Times.Once);
        }
        
        private class DummyCmd : AbstractCmd<CmdParametersSet,string>
        {
            public DummyCmd(IResultAppender<CmdParametersSet> resultAppender, CmdParametersSet args) : base(resultAppender, args)
            {
            }
                      
            protected override string RunCommand()
            {
                return "ran";
            }
        }


        [Test]
        public void Should_Call_IResultAppender_Append_Passing_Exception_As_Result_On_Every_Run_Call()
        {
            var appenderMock = new Mock<IResultAppender<CmdParametersSet>>();
            CmdParametersSet parameters = new CmdParametersSet();

            var cmd = new ExceptionRaiserCmd(appenderMock.Object, parameters);

            Assert.Throws<ArgumentException>(()=> cmd.Run());
            appenderMock.Verify(x => x.Append(parameters, It.IsAny<Exception>()), Times.Once);
        }

        private class ExceptionRaiserCmd : AbstractCmd<CmdParametersSet, string>
        {
            public ExceptionRaiserCmd(IResultAppender<CmdParametersSet> resultAppender, CmdParametersSet args) : base(resultAppender, args)
            {
            }

            protected override string RunCommand()
            {
                throw new ArgumentException();
            }
        }
    }
}
