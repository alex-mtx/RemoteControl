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
        private ICmdRepository<CmdParametersSet> _repository;

        public DapperOutput(ICmdRepository<CmdParametersSet> repository)
        {
            _repository = repository;
        }

        public void Send(CmdParametersSet context)
        {
            _repository.Update(context);
        }

    }
}
