using RC.Implementation.Receivers;
using RC.Interfaces.Receivers;
using System;

namespace RC.RabbitMQServices
{
    public class RabbitMQCmdReceiver : AbstractCmdReceiver
    {
        public override void StartReceiving(CmdReceivedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void StopReceiving()
        {
            throw new NotImplementedException();
        }
    }
}
