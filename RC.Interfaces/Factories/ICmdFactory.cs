using System.Collections.Generic;
using RC.Domain.Commands;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;

namespace RC.Interfaces.Factories
{
    public interface ICmdFactory<in TCmdParamSet> where TCmdParamSet : CmdParametersSet
    {
        ICmd Create(CmdType cmdType, TCmdParamSet paramsSet);
    }
}