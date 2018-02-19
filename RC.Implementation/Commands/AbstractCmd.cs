using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;

namespace RC.Implementation.Commands
{
    public abstract class AbstractCmd<TResult> : ICmd
    {
        protected readonly IResultAppender _resultAppender;
        private CmdParametersSet _paramSet;

        public AbstractCmd(IResultAppender resultAppender, CmdParametersSet args)
        {
            _resultAppender = resultAppender;
            _paramSet = args;
        }

      

        public virtual void Run()
        {
            TResult result;

            try
            {
                result = RunCommand();
                _paramSet.Status = CmdStatus.Executed;
            }
            catch (System.Exception)
            {
                _paramSet.Status = CmdStatus.ResultedInError;
                throw;
            }
            _resultAppender.Append(result);

        }

        protected abstract TResult RunCommand();
    }
}
