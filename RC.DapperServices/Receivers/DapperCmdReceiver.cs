using RC.Implementation.Commands;
using RC.Implementation.Receivers;
using RC.Interfaces.Factories;
using RC.Interfaces.Receivers;
using RC.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Timers;

namespace RC.DapperServices.Receivers
{
    public class DapperCmdReceiver : AbstractCmdReceiver
    {
        private readonly ICmdRepository<CmdParametersSet> _repository;
        private readonly int _intervalInSeconds;
        private readonly ICmdFactory<CmdParametersSet> _cmdFactory;
        private Timer _timer;

        public DapperCmdReceiver(int intervalInSeconds, ICmdFactory<CmdParametersSet> cmdFactory, ICmdRepository<CmdParametersSet> repository)
        {
            _intervalInSeconds = intervalInSeconds;
            _cmdFactory = cmdFactory;
            _repository = repository;
        }

       
        public override void StartReceiving(CmdReceivedEventHandler handler)
        {
            _timer = SetupScheduler(handler);
        }

        private Timer SetupScheduler(CmdReceivedEventHandler handler)
        {
            var timer = new Timer(_intervalInSeconds * 1000);
            timer.Elapsed += async (sender, e) => await FetchNewCmdRequestsAsync(handler);
            timer.AutoReset = true;
            timer.Enabled = true;
            return timer;
        }

        private async Task FetchNewCmdRequestsAsync(CmdReceivedEventHandler handler)
        {
            var newCmds = await Task.Run(() => _repository.PendingCommands());
            foreach (var cmdParam in newCmds)
            {
                var cmd = _cmdFactory.Create(cmdParam.CmdType, cmdParam);
                handler(cmd);
            }

        }
    }
}
