using RC.Domain.Commands;

namespace RC.Domain.Commands
{
    public class CmdResult<TResult, TCmdParamsSet> where TCmdParamsSet : CmdParametersSet
    {
        public CmdResult()
        {

        }
        public CmdResult(TResult result, TCmdParamsSet cmdParamsSet)
        {
            Result = result;
            CmdParamsSet = cmdParamsSet;
        }

        public TResult Result { get; set; }
        public TCmdParamsSet CmdParamsSet { get; set; }
    }

}
