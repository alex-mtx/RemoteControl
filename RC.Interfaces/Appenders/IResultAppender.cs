using RC.Domain.Commands;
using System;

namespace RC.Interfaces.Appenders
{
    public interface IResultAppender<in TParams>
        where TParams : CmdParametersSet
    {
        void Append<TResult>(TParams cmdParams, TResult result) ;
    }
}
