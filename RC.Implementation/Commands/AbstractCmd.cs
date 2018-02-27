using RC.Domain.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;

namespace RC.Implementation.Commands
{
    public abstract class AbstractCmd<TResult> : ICmd
    {
        protected readonly IResultAppender<CmdParametersSet> _resultAppender;
        private CmdParametersSet _paramSet;

        public AbstractCmd(IResultAppender<CmdParametersSet> resultAppender, CmdParametersSet args)
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
                _paramSet.Result = JsonServices.Json.Serialize(result);

            }
            catch (System.Exception e)
            {
                _paramSet.Status = CmdStatus.ResultedInError;
                _paramSet.Result = JsonServices.Json.Serialize(e);

                throw;
            }
            finally
            {

                _resultAppender.Append(_paramSet);
            }

            

        }

        protected abstract TResult RunCommand();
    }
}
