using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;

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

            }
            catch (System.Exception e)
            {
                _paramSet.Status = CmdStatus.ResultedInError;

                throw;
            }
            finally
            {
                _resultAppender.Append(_paramSet,result);
            }

            

        }

        protected abstract TResult RunCommand();
    }
}
