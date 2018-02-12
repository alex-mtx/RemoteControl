using RC.Interfaces.Commands;
using RC.Interfaces.Receivers;

namespace RC.Implementation.Receivers
{
    public abstract class AbstractCmdReceiver : ICmdReceiver
    {
        public abstract void StartReceiving(CmdReceivedEventHandler handler);
    }
}
