using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using System;
using System.Collections.Generic;

namespace RC.Implementation.Appenders
{
    public class GeneralResultAppender : IResultAppender<CmdParametersSet>
    {
        private IEnumerable<IOutput<CmdParametersSet>> _outputs;
        public GeneralResultAppender(IEnumerable<IOutput<CmdParametersSet>> outputs) => _outputs = outputs;

        public void Append(CmdParametersSet context)
        {
            foreach (var output in _outputs)
            {
                output.Send(context);
            }
        }
        
    }
}
