using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using System;

namespace RC.Implementation.Commands
{
    public class CmdParametersSet
    {
        public virtual Guid RequestId { get; set; }
        public virtual DateTime SentOn{ get; set; }

        public virtual CmdType CmdType { get; set; }
        public virtual bool Finished{ get; set; }



    }
}
