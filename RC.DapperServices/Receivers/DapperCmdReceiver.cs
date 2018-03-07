using RC.Domain.Commands;
using RC.Implementation.Receivers;
using RC.Interfaces.Factories;
using RC.Interfaces.Receivers;
using RC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Threading;
using System.Threading.Tasks;

namespace RC.DapperServices.Receivers
{
    public class DapperCmdReceiver : AbstractCmdReceiver
    {
        private readonly ICmdRepository<CmdParametersSet, CmdParametersSet> _repository;
        private readonly int _intervalInSeconds;
        private readonly ICmdFactory<CmdParametersSet> _cmdFactory;
        private bool Receive { get; set; }

        public DapperCmdReceiver(int intervalInSeconds, ICmdFactory<CmdParametersSet> cmdFactory, ICmdRepository<CmdParametersSet, CmdParametersSet> repository)
        {
            Receive = false;
            _intervalInSeconds = intervalInSeconds;
            _cmdFactory = cmdFactory;
            _repository = repository;
        }

       
        public override void StartReceiving(CmdReceivedEventHandler handler)
        {
            Receive = true;
            Task.Run(()=> FetchNewCmdRequests(handler));
        }

        private void FetchNewCmdRequests(CmdReceivedEventHandler handler)
        {
            IEnumerable<CmdParametersSet> newCmds = null;
            while (Receive)
            {
                try
                {
                    newCmds = _repository.PendingCommands();
                }
                catch (Exception)
                {
                    
                    throw;
                }

                DelegateCmds(handler, newCmds);
                Thread.Sleep(_intervalInSeconds * 1000);
            }

        }

        private void DelegateCmds(CmdReceivedEventHandler handler, IEnumerable<CmdParametersSet> newCmds)
        {
            foreach (var cmdParam in newCmds)
            {
                var cmd = _cmdFactory.Create(cmdParam.CmdType, cmdParam);
                try
                {
                    handler(cmd);

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public override void StopReceiving()
        {
            Receive = false;
        }
    }
}
