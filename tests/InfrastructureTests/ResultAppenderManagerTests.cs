using NUnit.Framework;
using RC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests
{
    [TestFixture(Category = "Appenders", TestOf = typeof(ResultAppenderManager))]

    public class ResultAppenderManagerTests
    {
        [Test]
        public void Is_Singleton()
        {
            var instance = ResultAppenderManager.Instance;
            var sameInstance = ResultAppenderManager.Instance;
            Assert.AreSame(instance, sameInstance);
        }
    }
}
