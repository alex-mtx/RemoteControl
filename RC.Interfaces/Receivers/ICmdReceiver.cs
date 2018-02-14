using RC.Interfaces.Commands;

namespace RC.Interfaces.Receivers
{
    public delegate void CmdReceivedEventHandler(ICmd cmd);

    public interface ICmdReceiver
    {
        void StartReceiving(CmdReceivedEventHandler handler);
        void StopReceiving();
    }
}