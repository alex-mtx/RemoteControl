using Moq;
using NUnit.Framework;
using RC.Domain.Commands;
using RC.Implementation.Appenders;
using RC.Interfaces.Appenders;
using System;
using System.Collections.Generic;

namespace ImplementationTests.Appenders
{
    [TestFixture(Category = "Appenders", TestOf = typeof(GeneralResultAppender))]
    public partial class GeneralResultAppenderTests
    {

        [Test]
        public void Should_Execute_Send_For_Each_Output()
        {
            //Arrange
            Mock<IOutput<CmdParametersSet>> outputMock = new Mock<IOutput<CmdParametersSet>>();
            var generalResultAppender = new GeneralResultAppender(new List<IOutput<CmdParametersSet>>() { outputMock.Object, outputMock.Object });
            var cmdResult = new CmdResult<string, CmdParametersSet>();

            //Act
            generalResultAppender.Append(new CmdParametersSet(),"");

            //Assert
            outputMock.Verify(x => x.Send(It.IsAny<CmdResult<string, CmdParametersSet>>()), Times.Exactly(2));
        }
    }
}
