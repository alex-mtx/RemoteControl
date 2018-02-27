using RC.Domain.Commands;
using RC.Implementation.Appenders;
using RC.Interfaces.Appenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.Infrastructure.Setup
{
    public class AppenderOutputSetup
    {
        public static IEnumerable<IOutput<CmdParametersSet>> RegisteredOutputs()
        {
            //TODO: get it from anywhere, e.g. app.config, database, just like log4net does
            return new List<IOutput<CmdParametersSet>>
            {
                new DebugConsoleOutput()
            };
        }
    }
}
