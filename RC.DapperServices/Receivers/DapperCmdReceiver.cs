using RC.Implementation.Commands;
using RC.Implementation.Receivers;
using RC.Interfaces.Factories;
using RC.Interfaces.Receivers;
using RC.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RC.DapperServices.Receivers
{
    public class DapperCmdReceiver : AbstractCmdReceiver
    {
        private readonly ICmdRepository<CmdParametersSet> _repository;
        private readonly int _intervalInSeconds;
        private readonly ICmdFactory<CmdParametersSet> _cmdFactory;
        private bool Receive { get; set; }

        public DapperCmdReceiver(int intervalInSeconds, ICmdFactory<CmdParametersSet> cmdFactory, ICmdRepository<CmdParametersSet> repository)
        {
            Receive = false;
            _intervalInSeconds = intervalInSeconds;
            _cmdFactory = cmdFactory;
            _repository = repository;
        }

       
        public override void StartReceiving(CmdReceivedEventHandler handler)
        {
            Receive = true;
            try
            {
                Task.Run(()=> FetchNewCmdRequests(handler));
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FetchNewCmdRequests(CmdReceivedEventHandler handler)
        {
            while (Receive)
            {
                try
                {
                    var newCmds =  _repository.PendingCommands();
                    foreach (var cmdParam in newCmds)
                    {
                        var cmd = _cmdFactory.Create(cmdParam.CmdType, cmdParam);
                        handler(cmd);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                Thread.Sleep(_intervalInSeconds * 1000);
            }

        }

        public override void StopReceiving()
        {
            Receive = false;
        }
    }
}
