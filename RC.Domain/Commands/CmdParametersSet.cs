
using System;

namespace RC.Domain.Commands
{
    public class CmdParametersSet
    {
        public virtual int Id { get; set; }
        public virtual Guid RequestId { get; set; }
        public virtual DateTime SentOn{ get; set; }

        public virtual CmdType CmdType { get; set; }
        public virtual CmdStatus Status{ get; set; }
        
    }
}
