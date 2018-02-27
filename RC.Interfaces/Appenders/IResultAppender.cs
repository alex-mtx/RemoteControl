using RC.Domain.Commands;
using System;

namespace RC.Interfaces.Appenders
{
    public interface IResultAppender<in TContext> where TContext : CmdParametersSet
    {
        void Append(TContext context);

    }
}
