using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using System;
using System.Diagnostics;

namespace RC.Implementation.Appenders
{
    public class DebugConsoleOutput : IOutput<CmdParametersSet>
    {
        public void Send<TResult>(CmdResult<TResult, CmdParametersSet> cmdResult)
        {
            Debug.WriteLine($"Context: {JsonServices.Json.Serialize(cmdResult)}");
        }
    }
}
