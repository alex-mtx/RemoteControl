using System.Collections.Generic;
using RC.Interfaces.Commands;

namespace RC.Interfaces.Factories
{
    public interface ICmdFactory
    {
        ICmd Create(CmdType cmdType, IDictionary<string, string> cmdParams);
    }
}