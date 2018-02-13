using System.Collections.Generic;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;

namespace RC.Interfaces.Factories
{
    public interface ICmdFactory<in TCmdParamSet>
    {
        ICmd Create(CmdType cmdType, TCmdParamSet paramsSet);
    }
}