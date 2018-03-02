using Moq;
using NUnit.Framework;
using RC.DapperServices.Appenders;
using RC.Domain.Commands;
using RC.Interfaces.Repositories;

namespace DapperServicesTests.Appenders
{
    [TestFixture(Category = "Appenders", TestOf = typeof(DapperOutput))]
    public class DapperOutputTests
    {
        [Test]
        public void Should_Call_Repository_Update_When_Send_Is_Called()
        {
            var repoMock = new Mock<ICmdRepository<CmdParametersSet, CmdParametersSet>>();
            var param = new CmdParametersSet();
            var output = new DapperOutput(repoMock.Object);
            var cmdResult = new CmdResult<string, CmdParametersSet>();
            output.Send(cmdResult);

            repoMock.Verify(x => x.Update(cmdResult));
        }

    }
}
