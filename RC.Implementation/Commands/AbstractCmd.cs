using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace RC.Implementation.Commands
{
    public abstract class AbstractCmd<TParamSet, TResult> : ICmd
    {
        protected readonly IResultAppender _resultAppender;
        TParamSet _paramSet;

        public AbstractCmd(IResultAppender resultAppender, TParamSet args)
        {
            _resultAppender = resultAppender;
            _paramSet = args;
        }

      

        public virtual void Run()
        {
            var result = RunCommand();
            _resultAppender.Append(result);

        }

        protected abstract TResult RunCommand();
    }
}
