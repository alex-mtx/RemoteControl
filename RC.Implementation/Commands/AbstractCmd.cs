using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using System;

namespace RC.Implementation.Commands
{
    public abstract class AbstractCmd<TParams,TResult> : ICmd 
        where TParams : CmdParametersSet
    {
        protected readonly IResultAppender<TParams> _resultAppender;
        protected TParams _paramSet;

        public AbstractCmd(IResultAppender<TParams> resultAppender, TParams args)
        {
            _resultAppender = resultAppender;
            _paramSet = args;
        }

        public virtual void Run()
        {
            TResult result = default(TResult);

            try
            {
                result = RunCommand();
                _paramSet.Status = CmdStatus.Executed;
                _resultAppender.Append(_paramSet, result);

            }
            catch (Exception e)
            {
                _paramSet.Status = CmdStatus.ResultedInError;
                _resultAppender.Append(_paramSet, e);

                throw;
            }

            

        }

        protected abstract TResult RunCommand();
    }
}
