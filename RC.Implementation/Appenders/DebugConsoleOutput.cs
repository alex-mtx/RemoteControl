using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using System;
using System.Diagnostics;

namespace RC.Implementation.Appenders
{
    public class DebugConsoleOutput : IOutput<CmdParametersSet>
    {
        public void Send(CmdParametersSet context) => Debug.WriteLine($"Context: {JsonServices.Json.Serialize(context)}");
    }
}
