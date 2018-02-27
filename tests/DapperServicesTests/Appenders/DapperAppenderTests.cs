using Moq;
using NUnit.Framework;
using RC.DapperServices.Appenders;
using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperServicesTests.Appenders
{
    [TestFixture(Category = "Appenders", TestOf = typeof(DapperOutput))]
    public class DapperAppenderTests
    {
        [Test]
        public void Should_Call_Repository_Update_When_Send_Is_Called()
        {
            var repoMock = new Mock<ICmdRepository<CmdParametersSet>>();
            var param = new CmdParametersSet();
            var output = new DapperOutput(repoMock.Object);

            output.Send(param);

            repoMock.Verify(x => x.Update(param));
        }
       

       
    }
}
