using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using System.Collections.Generic;

namespace RC.Implementation.Appenders
{
    public class GeneralResultAppender : IResultAppender<CmdParametersSet>
    {
        private IEnumerable<IOutput<CmdParametersSet>> _outputs;
        public GeneralResultAppender(IEnumerable<IOutput<CmdParametersSet>> outputs) => _outputs = outputs;

        public void Append<TResult>(CmdParametersSet cmdParams, TResult result)
        {
            var cmdResult = new CmdResult<TResult, CmdParametersSet>();
            foreach (var output in _outputs)
            {
                cmdResult.Result = result;
                cmdResult.CmdParamsSet= cmdParams;
                output.Send(cmdResult);
            }
        }
    }
}
