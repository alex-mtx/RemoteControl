using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.DapperServices.Appenders
{
    public class DapperOutput : IOutput<CmdParametersSet>
    {
        private ICmdRepository<CmdParametersSet, CmdParametersSet> _repository;

        public DapperOutput(ICmdRepository<CmdParametersSet, CmdParametersSet> repository)
        {
            _repository = repository;
        }

        public void Send<TResult>(CmdResult<TResult, CmdParametersSet> cmdResult)
        {
            _repository.Update(cmdResult);

        }
    }
}
