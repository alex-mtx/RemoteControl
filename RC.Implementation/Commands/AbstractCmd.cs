using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;

namespace RC.Implementation.Commands
{
    public abstract class AbstractCmd<TParamSet, TResult> where TParamSet : CmdParametersSet,ICmd
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
