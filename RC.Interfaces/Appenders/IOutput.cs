using RC.Domain.Commands;
using System;

namespace RC.Interfaces.Appenders
{
    public interface IOutput<in TContext> where TContext : CmdParametersSet
    {
        void Send(TContext context);
    }
}
