using RC.Domain.Commands;

namespace RC.Domain.Commands
{
    public class CmdResult<TResult, TCmdParamsSet> where TCmdParamsSet : CmdParametersSet
    {
        public CmdResult(TResult result, TCmdParamsSet cmdParamsSet)
        {
            Result = result;
            CmdParamsSet = cmdParamsSet;
        }

        public virtual TResult Result { get; set; }
        public virtual TCmdParamsSet CmdParamsSet { get; set; }
    }

}
