using System.Collections.Generic;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;

namespace RC.Interfaces.Factories
{
    public interface ICmdFactory
    {
        ICmd Create(CmdType cmdType, IDictionary<string, string> cmdParams);
    }
}